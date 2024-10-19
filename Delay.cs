using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class Delay
    {
        private readonly ILogger<Delay> _logger;

        public Delay(ILogger<Delay> logger)
        {
            _logger = logger;
        }

        [Function("Delay")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            var msValue = req.Query["ms"].FirstOrDefault();
            if (string.IsNullOrEmpty(msValue))
            {
                return new BadRequestObjectResult("Invalid 'ms' value on the query string");
            }

            var durationMilliseconds = int.Parse(msValue);
            _logger.LogInformation($"Delay duration: {durationMilliseconds} ms");
            System.Threading.Thread.Sleep(durationMilliseconds);
            return new OkObjectResult($"Delay duration: {durationMilliseconds} ms");
        }
    }
}
