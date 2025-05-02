using FoodFlowSystem.DTOs;
using System.Net;
using System.Text.Json;

namespace FoodFlowSystem.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ExceptionMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, logger);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ExceptionMiddleware> logger)
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
            object errors = (exception is ApiException apiEx && apiEx.Errors != null) ? apiEx.Errors : null;

            var response = new
            {
                statusCode,
                message,
                errors,
            };

            logger.LogError(message);
            logger.LogError(exception, "An error occurred while processing the request.");

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var jsonResponse = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(jsonResponse);
        }

    }
}
