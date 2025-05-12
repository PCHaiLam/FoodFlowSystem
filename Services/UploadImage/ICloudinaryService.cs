namespace FoodFlowSystem.Services.UploadImage
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
