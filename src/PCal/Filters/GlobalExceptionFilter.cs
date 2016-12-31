using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PCal.Responses;

namespace PCal.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter, IDisposable
    {
        private readonly ILogger _logger;

        public GlobalExceptionFilter(ILoggerFactory logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _logger = logger.CreateLogger("Global Exception Filter");
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
        }

        public void OnException(ExceptionContext context)
        {
            var response = new ErrorResponse {Message = context.Exception.Message};

#if DEBUG
            response.StackTrace = context.Exception.StackTrace;
#endif

            context.Result = new ObjectResult(response)
            {
                StatusCode = GetHttpStatusCode(context.Exception),
                DeclaredType = typeof(ErrorResponse)
            };
            _logger.LogError("GlobalExceptionNFilter", context.Exception);
        }

        private bool _disposed;

        private static int GetHttpStatusCode(Exception ex)
        {
            if (ex is HttpResponseException)
                return (int) (ex as HttpResponseException).HttpStatusCode;

            return (int) HttpStatusCode.InternalServerError;
        }
    }
}