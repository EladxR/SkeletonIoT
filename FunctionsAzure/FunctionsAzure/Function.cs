using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;


namespace CSharp
{
    public static class Function
    {
        private static HttpClient httpClient = new HttpClient();
        private static string Etag = string.Empty;
        private static int counter= 0;

        [FunctionName("index")]
        public static IActionResult GetHomePage([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req, ExecutionContext context)
        {
            var path = Path.Combine(context.FunctionAppDirectory, "content", "index.html");
            return new ContentResult
            {
                Content = File.ReadAllText(path),
                ContentType = "text/html",
            };
        }

        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
            [SignalRConnectionInfo(HubName = "serverless")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("broadcast")]
        public static async Task Broadcast([TimerTrigger("*/1 * * * * *")] TimerInfo myTimer,
        [SignalR(HubName = "serverless")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://eladskeletonfunctionapp.azurewebsites.net/api/GetCounter?code=b0Z/mtOklciCPCer4S5KjrSPxYdgvzyxhASDiZznehw4Gcu0SddLjQ==");
            request.Headers.UserAgent.ParseAdd("Serverless");
            request.Headers.Add("If-None-Match", Etag);
            var response = await httpClient.SendAsync(request);
            if (response.Headers.Contains("Etag"))
            {
                Etag = response.Headers.GetValues("Etag").First();
            }
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                counter = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
            }

            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "newMessage",
                    Arguments = new[] { $"{counter}" }
                });


            /*string value = req.Query["value"];
            if (value == null)
                return new BadRequestResult();
            log.LogInformation("value is "+value);

            counter = Int32.Parse(value);
            log.LogInformation("counter is"+counter);

            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "newMessage",
                    Arguments = new[] { $"counter: {counter}" }
                });
            log.LogInformation("succefully got to the end of broadcast");

            return new OkObjectResult($"Hello, {value}");*/

        }

        private class GitResult
        {
            [JsonRequired]
            [JsonProperty("stargazers_count")]
            public int counter { get; set; }
        }
    }
}