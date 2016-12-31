using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace PCal.Filters
{
    public class GlobalActionFilter:ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public GlobalActionFilter(ILoggerFactory logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger.CreateLogger("Global Action Filter");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _logger.LogInformation("Global Action Filter - OnActionExecuting");
        }
    }
}