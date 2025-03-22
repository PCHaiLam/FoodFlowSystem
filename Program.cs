using FluentValidation;
using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities;
using FoodFlowSystem.Helpers;
using FoodFlowSystem.Interceptors;
using FoodFlowSystem.Mappers;
using FoodFlowSystem.Middlewares;
using FoodFlowSystem.Repositories;
using FoodFlowSystem.Repositories.Auth;
using FoodFlowSystem.Repositories.Category;
using FoodFlowSystem.Repositories.Feedback;
using FoodFlowSystem.Repositories.OAuth;
using FoodFlowSystem.Repositories.Order;
using FoodFlowSystem.Repositories.OrderItem;
using FoodFlowSystem.Repositories.Payment;
using FoodFlowSystem.Repositories.Product;
using FoodFlowSystem.Repositories.ProductVersion;
using FoodFlowSystem.Repositories.Table;
using FoodFlowSystem.Repositories.User;
using FoodFlowSystem.Services.Auth;
using FoodFlowSystem.Services.Category;
using FoodFlowSystem.Services.Feedback;
using FoodFlowSystem.Services.Order;
using FoodFlowSystem.Services.Payment;
using FoodFlowSystem.Services.Product;
using FoodFlowSystem.Services.Table;
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

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// JWT Config
builder.Services.Configure<JwtSettingClass>(builder.Configuration.GetSection("JwtSettings"));

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

// Dependency Injection - Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

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
            context.HandleResponse();
            context.Response.StatusCode = 401;
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

var app = builder.Build();

// Middleware Pipeline Configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Middlewares
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ApiResponseMiddleware>();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("auth_token", "refresh_token", "server", "date");
});

app.MapControllers();

app.Run();

