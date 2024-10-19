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
            var durationSeconds = req.Query["duration"].FirstOrDefault();
            if (string.IsNullOrEmpty(durationSeconds))
            {
                return new BadRequestObjectResult("Invalid 'duration' value on the query string");
            }

            var duration = TimeSpan.FromSeconds(int.Parse(durationSeconds));
            _logger.LogInformation($"Delay duration: {duration}");
            System.Threading.Thread.Sleep(duration);
            return new OkObjectResult($"Delay duration: {duration}");
        }
    }
}
