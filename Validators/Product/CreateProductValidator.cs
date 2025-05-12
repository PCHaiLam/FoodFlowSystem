using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Product;

namespace FoodFlowSystem.Validators.Product
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            //chuyển thành tiếng việt hết
            //RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            //RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required");
            //RuleFor(x => x.UrlImage).NotEmpty().WithMessage("ImageUrl is required");
            //RuleFor(x => x.CategoryID).NotEmpty().WithMessage("CategoryID is required");
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tên sản phẩm là bắt buộc.")
                .Length(3, 50)
                .WithMessage("Tên sản phẩm phải có độ dài từ 3 đến 50 ký tự.");
            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Giá sản phẩm là bắt buộc.")
                .GreaterThan(0)
                .WithMessage("Giá sản phẩm phải lớn hơn 0.");
            RuleFor(x => x.ImageFiles)
                .NotEmpty()
                .WithMessage("Ảnh sản phẩm là bắt buộc.")
                .Must(file => file.Length > 0)
                .WithMessage("Ảnh sản phẩm không hợp lệ.")
                .Must(file => file.ContentType == "image/jpeg" || file.ContentType == "image/png")
                .WithMessage("Chỉ chấp nhận định dạng ảnh JPEG hoặc PNG.");
            RuleFor(x => x.CategoryID)
                .NotEmpty()
                .WithMessage("Loại sản phẩm là bắt buộc.")
                .Must(id => int.TryParse(id.ToString(), out _))
                .WithMessage("ID danh mục không hợp lệ.");
        }
    }
}
