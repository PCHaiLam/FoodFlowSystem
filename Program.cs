using FluentValidation;
using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities;
using FoodFlowSystem.Interceptors;
using FoodFlowSystem.Mappers;
using FoodFlowSystem.Middlewares;
using FoodFlowSystem.Repositories;
using FoodFlowSystem.Repositories.User;
using FoodFlowSystem.Services.User;
using FoodFlowSystem.Validators.User;
using Microsoft.EntityFrameworkCore;

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
//User
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserValidator>();


// Dependency Injection - Repositories
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Dependency Injection - Services
builder.Services.AddScoped<IUserService, UserService>();

// JWT 

// Authentication

// Register HttpClient
builder.Services.AddHttpClient();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(UserMapper));

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

