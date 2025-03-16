using FluentValidation;
using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities;
using FoodFlowSystem.Helpers;
using FoodFlowSystem.Interceptors;
using FoodFlowSystem.Mappers;
using FoodFlowSystem.Middlewares;
using FoodFlowSystem.Repositories;
using FoodFlowSystem.Repositories.Auth;
using FoodFlowSystem.Repositories.OAuth;
using FoodFlowSystem.Repositories.User;
using FoodFlowSystem.Services.Auth;
using FoodFlowSystem.Services.User;
using FoodFlowSystem.Validators.Auth;
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


// Dependency Injection - Repositories
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IOAuthRepository, OAuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Dependency Injection - Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

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

