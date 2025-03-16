using FoodFlowSystem.Middlewares.Exceptions;
using System.Net;

namespace FoodFlowSystem.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode = exception switch
            {
                ApiException ex => ex.StatusCode,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                ArgumentException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                NotImplementedException => StatusCodes.Status501NotImplemented,
                _ => StatusCodes.Status500InternalServerError
            };

            string message = exception switch
            {
                ApiException ex => ex.Message,
                UnauthorizedAccessException => "Unauthorized access.",
                ArgumentException => $"Invalid argument: {exception.Message}",
                KeyNotFoundException => "Resource not found.",
                NotImplementedException => "This feature is not implemented yet.",
                _ => "An unexpected error occurred. Please try again later."
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                statusCode,
                message
            };

            await context.Response.WriteAsJsonAsync(response);
        }

    }
}
