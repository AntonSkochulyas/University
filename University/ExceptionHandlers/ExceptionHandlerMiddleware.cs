using System.Net;

namespace University.ExceptionHandlers
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception = : {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = "application/json";

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "Internal Server Error";

            // Custom exceptions
            if (ex is ArgumentNullException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message = "Required parameter is missing";
            }
            else if (ex is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                message = "Access denied";
            }

            httpContext.Response.StatusCode = statusCode;

            var exceptionDetails = new ExceptionDetails
            {
                StatusCode = statusCode,
                Message = message
            };

            return httpContext.Response.WriteAsync(exceptionDetails.ToString());
        }
    }
}
