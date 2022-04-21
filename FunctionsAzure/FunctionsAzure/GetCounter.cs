using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;



namespace Company.Function
{
    public class LogEntity : TableEntity
    {
        public string OriginalName { get; set; }
        public int Value { get; set; }

    }
    public class Entity : TableEntity
    {
        public string Value { get; set; }

    }
    public static class GetCounter
    {
        [FunctionName("GetCounter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Table("skeletonTable","Counter","1", Connection = "AzureWebJobsStorage")] Entity myEntity,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            
            return new OkObjectResult(myEntity.Value);
        }
        /* public static void Run(string myQueueItem, MyTable counter, ILogger log)
         {
             log.LogInformation($"Counter value is: {counter.Value}");
         }*/
        /*public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            MyTable counter, ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            int value = counter.Value;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string s_value = value.ToString();

            string responseMessage = string.IsNullOrEmpty(s_value)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {s_value}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }*/

    }

}