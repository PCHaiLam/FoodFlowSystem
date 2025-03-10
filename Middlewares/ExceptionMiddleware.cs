using FoodFlowSystem.Middlewares.Exceptions;
using System.Net;
using System.Text.Json;

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

            int statusCode = StatusCodes.Status500InternalServerError;
            string message = "An unexpected error occurred. Please try again later.";

            if (exception is ApiException apiException)
            {
                statusCode = apiException.StatusCode;
                message = apiException.Message;
            }

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
