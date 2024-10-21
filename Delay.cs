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
            var name = req.Query["name"];
            if (string.IsNullOrEmpty(name))
            {
                return new BadRequestObjectResult("Please pass a name on the query string");
            }

            var durationSeconds = req.Query["duration"].FirstOrDefault();
            if (string.IsNullOrEmpty(durationSeconds))
            {
                return new BadRequestObjectResult("Invalid 'duration' value on the query string");
            }

            var seconds = int.Parse(durationSeconds);
            _logger.LogInformation($"{name}: Delay duration: {seconds} seconds");

            for (int i = 0; i < seconds; i++)
            {
                _logger.LogInformation($"{name}: {i} (PID: {Environment.ProcessId})");
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            
            return new OkObjectResult($"{name}: Delay duration: {seconds} seconds");
        }
    }
}
