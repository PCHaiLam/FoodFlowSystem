using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FoodFlowSystem.DTOs;
using Microsoft.Extensions.Options;

namespace FoodFlowSystem.Services.UploadImage
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            var cloudinaryAccount = new Account(
                               options.Value.CloudName,
                                              options.Value.ApiKey,
                                                             options.Value.ApiSecret
                                                                        );

            _cloudinary = new Cloudinary(cloudinaryAccount);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            try
            {
                await using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "products",
                    PublicId = $"product_{Guid.NewGuid()}",
                    Overwrite = true,
                    Transformation = new Transformation().Quality("auto").FetchFormat("auto")
                };

                var imageData = await _cloudinary.UploadAsync(uploadParams);

                if (imageData.Error != null)
                {
                    throw new ApiException("Lỗi khi tải ảnh lên.", 500);
                }
                
                var result = imageData.SecureUrl.ToString();
                return result;
            }
            catch (Exception ex)
            {
                throw new ApiException("Lỗi khi lưu ảnh.", 500, ex);
            }
        }
    }
}
