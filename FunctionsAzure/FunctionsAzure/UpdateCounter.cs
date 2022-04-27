using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Azure;
using System.Net.Http;



namespace FunctionsAzure
{
    public static class UpdateCounter
    {
        private static readonly HttpClient client = new HttpClient();

        [FunctionName("UpdateCounter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get","post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string value = req.Query["value"];
            if (value == null)
                return new BadRequestResult();

            var tableClient = new TableClient(
                new Uri("https://eyg.table.core.windows.net/"),
                "skeletonTable",
                new TableSharedKeyCredential("eyg", "BouRAn3sqA5Lv/93VJGyVPUFoPOz70gL5Z3jGQOlHS6rEsPgWtXXUZvXFMPoMEc8zTPt1YXJbGfT+AStmNfTjQ=="));

         
            String partitionKey = "Counter";
            String rowKey = "1";

            TableEntity tableEntity = new TableEntity(partitionKey, rowKey);
            tableEntity.Add("Value", value);
            tableEntity.Add("Timestamp", DateTime.Now);


            await tableClient.UpdateEntityAsync(tableEntity, Azure.ETag.All ,TableUpdateMode.Replace);

            //trigger broadcast signalR
            var response = await client.PostAsync("https://eladskeletonfunctionapp.azurewebsites.net/api/broadcast?value=" + value, null);
            var responseStringUpdate = await response.Content.ReadAsStringAsync();
            log.LogInformation(responseStringUpdate);


            return new OkObjectResult($"Hello, {value}");
        }

       
    }

}
