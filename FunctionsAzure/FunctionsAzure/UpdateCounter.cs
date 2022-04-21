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


namespace FunctionsAzure
{
    public static class UpdateCounter
    {
    
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

            
             return new OkObjectResult($"Hello, {value}");
        }

       
    }

}
