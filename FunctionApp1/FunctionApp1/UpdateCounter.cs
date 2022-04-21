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

namespace FunctionApp1
{
    public static class UpdateCounter
    {
        public class Entity : TableEntity
        {
            public int Value { get; set; }

        }

        [FunctionName("UpdateCounter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Table("skeletonTable")] CloudTable table,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string value = req.Query["value"];
            if (value == null)
            {
                return new BadRequestResult();
            }

            Entity entity = new Entity { Value = Int32.Parse(value), RowKey = "1", PartitionKey = "Counter" };
            // Create the InsertOrReplace table operation
            TableOperation updateOperation = TableOperation.Replace(entity);

            // Execute the operation.
            TableResult updateResult = await table.ExecuteAsync(updateOperation);



            return new OkObjectResult($"Hello, {value}");
        }
    }
}
