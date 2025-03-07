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

            string message;
            switch (exception)
            {
                case NotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    message = exception.Message;
                    break;
                case AuthenticationException:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    message = exception.Message;
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    message = "An unexpected error occurred. Please try again later.";
                    break;
            }
            var response = new
            {
                message = message,
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
