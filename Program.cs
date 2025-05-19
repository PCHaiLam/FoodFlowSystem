using FluentValidation;
using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.DTOs;
using FoodFlowSystem.DTOs.Requests.Payment.PaymentConfigs;
using FoodFlowSystem.Helpers;
using FoodFlowSystem.Interceptors;
using FoodFlowSystem.Mappers;
using FoodFlowSystem.Middlewares;
using FoodFlowSystem.Repositories;
using FoodFlowSystem.Repositories.Auth;
using FoodFlowSystem.Repositories.Category;
using FoodFlowSystem.Repositories.EmailTemplates;
using FoodFlowSystem.Repositories.Feedback;
using FoodFlowSystem.Repositories.Invoice;
using FoodFlowSystem.Repositories.OAuth;
using FoodFlowSystem.Repositories.Order;
using FoodFlowSystem.Repositories.OrderItem;
using FoodFlowSystem.Repositories.Payment;
using FoodFlowSystem.Repositories.Product;
using FoodFlowSystem.Repositories.ProductVersion;
using FoodFlowSystem.Repositories.Statistic;
using FoodFlowSystem.Repositories.Table;
using FoodFlowSystem.Repositories.Token;
using FoodFlowSystem.Repositories.User;
using FoodFlowSystem.Services.Auth;
using FoodFlowSystem.Services.Category;
using FoodFlowSystem.Services.Feedback;
using FoodFlowSystem.Services.Invoice;
using FoodFlowSystem.Services.Order;
using FoodFlowSystem.Services.Payment;
using FoodFlowSystem.Services.Product;
using FoodFlowSystem.Services.Recommendations;
using FoodFlowSystem.Services.SendMail;
using FoodFlowSystem.Services.Statistic;
using FoodFlowSystem.Services.Table;
using FoodFlowSystem.Services.UploadImage;
using FoodFlowSystem.Services.User;
using FoodFlowSystem.Validators.Auth;
using FoodFlowSystem.Validators.Category;
using FoodFlowSystem.Validators.Feedback;
using FoodFlowSystem.Validators.Order;
using FoodFlowSystem.Validators.OrderItem;
using FoodFlowSystem.Validators.Payment;
using FoodFlowSystem.Validators.Product;
using FoodFlowSystem.Validators.Table;
using FoodFlowSystem.Validators.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Base Config
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    }); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder
            .WithOrigins("http://localhost:5173")
            //.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("auth_token", "refresh_token");
        });
});

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Config
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<SendMailConfig>(builder.Configuration.GetSection("SendMailConfig"));
builder.Services.Configure<VNPayConfig>(builder.Configuration.GetSection("PaymentGateways:VNPayConfig"));
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

//Interceptors
builder.Services.AddSingleton<AuditLogInterceptor>();
builder.Services.AddSingleton<TimeInterceptor>();

//DbContext
builder.Services.AddDbContext<MssqlDbContext>((serviceProvider, options) =>
{
    var timeInterceptor = serviceProvider.GetRequiredService<TimeInterceptor>();
    var interceptor = serviceProvider.GetRequiredService<AuditLogInterceptor>();

    options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlString"))
           .AddInterceptors(interceptor, timeInterceptor)
           .EnableSensitiveDataLogging();
    //.LogTo(Console.WriteLine);
});

//Validators
//Auth
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
//User
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserValidator>();
//Category
builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryValidator>();
//Product
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductValidator>();
//Order
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateOrderValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderItemValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateOrderItemValidator>();
//Table
builder.Services.AddValidatorsFromAssemblyContaining<CreateTableValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateTableValidator>();
//Feedback
builder.Services.AddValidatorsFromAssemblyContaining<UpdateFeedbackValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateFeedbackValidator>();
//Payment
builder.Services.AddValidatorsFromAssemblyContaining<CreatePaymentValidator>();
//Invoice


// Dependency Injection - Repositories
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IOAuthRepository, OAuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductVersionRepository, ProductVersionRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IEmailTemplatesRepository, EmailTemplatesRepository>();
builder.Services.AddScoped<IStatisticRepository, StatisticRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

// Dependency Injection - Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IVNPayService, VNPayService>();
builder.Services.AddScoped<ISendMailService, SendMailService>();
builder.Services.AddScoped<IRecommendationsService, RecommendationsService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IStatisticService, StatisticService>();

// JWT 
builder.Services.AddScoped<JwtHelper>();

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(ops =>
{
    ops.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };

    ops.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            Console.WriteLine("OnChallenge: " + context.Error);
            context.HandleResponse();
            context.Response.StatusCode = 401;
            //context.Response.Headers.Append("StatusCode", "401");
            context.Response.ContentType = "application/json";

            var result = JsonConvert.SerializeObject(new
            {
                statusCode = 401,
                message = "Unauthorized"
            });

            return context.Response.WriteAsync(result);
        }
    };
});

// Register HttpClient
builder.Services.AddHttpClient();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(UserMapper));
builder.Services.AddAutoMapper(typeof(ProductMapper));
builder.Services.AddAutoMapper(typeof(OrderMapper));
builder.Services.AddAutoMapper(typeof(OrderItemMapper));
builder.Services.AddAutoMapper(typeof(CategoryMapper));
builder.Services.AddAutoMapper(typeof(TableMapper));
builder.Services.AddAutoMapper(typeof(FeedbackMapper));
builder.Services.AddAutoMapper(typeof(PaymentMapper));
builder.Services.AddAutoMapper(typeof(InvoiceMapper));

var app = builder.Build();

// Middleware Pipeline Configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

// Middlewares
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ApiResponseMiddleware>();

app.MapControllers();

app.Run();

