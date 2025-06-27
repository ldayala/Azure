using Azure.Application.Abstractions;
using FluentValidation;

namespace Azure.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _hostEnvironment;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment hostEnvironment)
        {
            _next = next;
            _logger = logger;
            _hostEnvironment = hostEnvironment; // You can inject this via DI if needed

        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This error is a Exception");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                object errorResponse = null;

                if (ex is ValidationException validationException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    errorResponse = validationException.Errors;
                }
                else
                {
                    errorResponse = new Error("UnexpectedError",
                           _hostEnvironment.IsDevelopment()
                           ? ex.ToString()
                           : "An unexpected error occurred. Please try again later."
                           );
                }

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
