using FoodFlowSystem.DTOs;
using FoodFlowSystem.DTOs.Responses;
using System.Text;
using System.Text.Json;

public class ApiResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JsonSerializerOptions _jsonOptions;

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
                    bool hasStandardFormat = responseBody.Contains("\"data\":") &&
                                          (responseBody.Contains("\"pagination\":") || !context.Items.ContainsKey("RequiresPagination"));
                    // Kiểm tra xem đã có định dạng data chưa
                    if (!hasStandardFormat)
                    {
                        // Đọc dữ liệu từ response gốc
                        var originalData = JsonSerializer.Deserialize<object>(responseBody);
                        object formattedResponse;

                        // Kiểm tra nếu cần pagination
                        if (context.Items.ContainsKey("PaginationInfo") && context.Items["PaginationInfo"] != null)
                        {
                            var paginationInfo = (PaginationInfo)context.Items["PaginationInfo"];
                            formattedResponse = new { data = originalData, pagination = paginationInfo };
                        }
                        else
                        {
                            formattedResponse = new { data = originalData };
                        }

                        // Serializer lại theo đúng format
                        var jsonResponse = JsonSerializer.Serialize(formattedResponse, _jsonOptions);

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