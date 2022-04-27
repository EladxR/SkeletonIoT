using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace FunctionsAzure
{
    public class MessageReceiver
    {
        private static readonly HttpClient client = new HttpClient();
        [FunctionName("MessageReceiver")]
        public static async Task Run(
    [EventHubTrigger("skeletonevenhub", Connection = "EventHubConnectionAppSetting")] EventData myEventHubMessage,
    DateTime enqueuedTimeUtc,
    Int64 sequenceNumber,
    string offset,
    ILogger log)
        {

            string responseString = await client.GetStringAsync("https://eladskeletonfunctionapp.azurewebsites.net/api/GetCounter?code=b0Z/mtOklciCPCer4S5KjrSPxYdgvzyxhASDiZznehw4Gcu0SddLjQ==");
            int newValue = Int32.Parse(responseString) + 1; // updating the value
            var response = await client.PostAsync("https://eladskeletonfunctionapp.azurewebsites.net/api/UpdateCounter?value=" + newValue, null);
            var responseStringUpdate = await response.Content.ReadAsStringAsync();
            log.LogInformation(responseStringUpdate);




            // var exceptions = new List<Exception>();

            /*foreach (EventData eventData in events)
            {
                try
                {
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                    // Replace these two lines with your processing logic.
                    log.LogInformation($"C# Event Hub trigger function processed a message: {messageBody}");

                    if (messageBody.Equals("1"))
                    {
                        log.LogInformation("you have clicked 1");
                        string responseString = await client.GetStringAsync("https://eladskeletonfunctionapp.azurewebsites.net/api/GetCounter?code=b0Z/mtOklciCPCer4S5KjrSPxYdgvzyxhASDiZznehw4Gcu0SddLjQ==");
                        int newValue = Int32.Parse(responseString)+1; // updating the value
                        var response = await client.PostAsync("https://eladskeletonfunctionapp.azurewebsites.net/api/UpdateCounter?value="+newValue, null);
                        var responseStringUpdate = await response.Content.ReadAsStringAsync();
                        log.LogInformation(responseStringUpdate);

                    }
                    else
                    {
                        log.LogInformation("you have clicked 2");
                    }
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();*/
        }
    }
}
