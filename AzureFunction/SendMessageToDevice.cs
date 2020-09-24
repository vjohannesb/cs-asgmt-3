using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedLibrary.Models;
using SharedLibrary.Services;
using Microsoft.Azure.Devices;
using SharedLibrary;

namespace AzureFunction
{
    public static class SendMessageToDevice
    {
        private static readonly ServiceClient serviceClient = 
            ServiceClient.CreateFromConnectionString(Config.IotHubConnectionString);

        [FunctionName("SendMessageToDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // Logga i Azure Functions konsol
            log.LogInformation("C# HTTP trigger function processed a request.");

            // QueryString
            string targetDeviceId = req.Query["targetdeviceid"];
            string message = req.Query["message"];

            // HTTP-Body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<MessageModel>(requestBody);

            // Om QueryString = null, använd HTTP-Body
            targetDeviceId = targetDeviceId ?? data?.TargetDeviceID;
            message = message ?? data?.Message;

            if (string.IsNullOrEmpty(targetDeviceId) || string.IsNullOrEmpty(message))
                return new BadRequestObjectResult("HTTP triggered function executed, but no target device id or message was supplied. Valid parameters are 'targetdeviceid' & 'message'.");

            await DeviceService.SendMessageToDeviceAsync(serviceClient, targetDeviceId, message);
            return new OkObjectResult($"HTTP triggered function executed successfully. Message sent to '{targetDeviceId}'.");
        }
    }
}
