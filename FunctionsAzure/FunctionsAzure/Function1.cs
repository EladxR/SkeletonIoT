using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace azureFunctions
{
    public static class GetCounter
    {
        public class SkeletonTable
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public int Value { get; set; }
        }


        [FunctionName("GetCounter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
          //  [Table("SkeletonTable", "Counter", "1",Connection = "DefaultEndpointsProtocol=https;AccountName=eyg;AccountKey=BouRAn3sqA5Lv/93VJGyVPUFoPOz70gL5Z3jGQOlHS6rEsPgWtXXUZvXFMPoMEc8zTPt1YXJbGfT+AStmNfTjQ==;EndpointSuffix=core.windows.net")] SkeletonTable Counter,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


           // string responseMessage = Counter.Value.ToString();


            return new OkObjectResult("eyg");
        }
        /* [FunctionName("TableInput")]
         public static int TableInput(
         [QueueTrigger("table-items")] string input,
         [Table("SkeletonTable", "Counter", "1")] SkeletonTable Counter,
         ILogger log)
         {
             return Counter.Value;
         }*/
    }
}