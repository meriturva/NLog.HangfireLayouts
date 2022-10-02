using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Sample.Jobs
{
    public class SimpleJob
    {
        private ILogger<SimpleJob> _logger;

        public SimpleJob(ILogger<SimpleJob> logger)
        {
            _logger = logger;
        }

        public async Task DoJobAsync()
        {
            _logger.LogInformation("Test log message");
            
            await Task.CompletedTask;
        }
    }
}
