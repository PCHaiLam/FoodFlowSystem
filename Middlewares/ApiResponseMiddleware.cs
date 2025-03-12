using FoodFlowSystem.DTOs.Responses;
using System.Text;
using System.Text.Json;

public class ApiResponseMiddleware
{
    private readonly RequestDelegate _next;

    public ApiResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        try
        {
            await _next(context);

            if (context.Response.StatusCode >= 200 
                && context.Response.StatusCode < 300
                && context.Response.ContentType?.Contains("application/json") == true)
            {
                memoryStream.Position = 0;
                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                if (!string.IsNullOrEmpty(responseBody))
                {
                    // Kiểm tra xem đã có định dạng data chưa
                    if (!responseBody.Contains("\"data\":"))
                    {
                        // Chuyển đổi response gốc thành đối tượng
                        var originalData = JsonSerializer.Deserialize<object>(responseBody);

                        // Tạo response mới với cấu trúc data
                        var jsonResponse = JsonSerializer.Serialize(new { data = originalData });

                        // Ghi lại vào response stream
                        memoryStream.SetLength(0);
                        await memoryStream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                        await memoryStream.FlushAsync();
                    }
                }
            }

            memoryStream.Position = 0;
            await memoryStream.CopyToAsync(originalBodyStream);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
            await memoryStream.DisposeAsync();
        }
    }
}