using Microsoft.AspNetCore.Mvc.Filters;

namespace CSharp11WebAPIDemo.Infrastructure
{
    public class LoggingFilter : IAsyncActionFilter
    {
        public ILogger<LoggingFilter> _logger;

        public LoggingFilter(ILogger<LoggingFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("Action starts");
            await next();
            _logger.LogInformation("Action ends");
        }
    }
    
}
