using FluentValidation;
using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.DTOs.Requests.Auth;
using FoodFlowSystem.Entities;
using FoodFlowSystem.Helpers;
using FoodFlowSystem.Interceptors;
using FoodFlowSystem.Mappers;
using FoodFlowSystem.Middlewares;
using FoodFlowSystem.Repositories;
using FoodFlowSystem.Repositories.Auth;
using FoodFlowSystem.Services.Auth;
using FoodFlowSystem.Validators.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//add log
//builder.Services.AddDbContext<MssqlDbContext>(options =>
//    options.UseSqlServer("MsSqlString")
//           .EnableSensitiveDataLogging()
//           .LogTo(Console.WriteLine)
//);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add Interceptors
builder.Services.AddSingleton<AuditLogInterceptor>();
builder.Services.AddSingleton<TimeInterceptor>();

// Add DbContext
builder.Services.AddDbContext<MssqlDbContext>((serviceProvider, options) =>
{
    var timeInterceptor = serviceProvider.GetRequiredService<TimeInterceptor>();
    var interceptor = serviceProvider.GetRequiredService<AuditLogInterceptor>();
    options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlString"));
});

//Configuration
builder.Services.Configure<JwtSettingClass>(builder.Configuration.GetSection("JwtSettings"));

//JWT
builder.Services.AddScoped<JwtHelper>();

//Validators
//user
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>();


//Dependency Injection

//Repositories
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IAuthRepository, AuthRepository>();


//Services
builder.Services.AddScoped<IAuthService, AuthService>();

//Authentication
builder.Services.AddAuthentication(options =>
{
    //jwt
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(ops =>
{
    //ops.RequireHttpsMetadata = false;
    //ops.SaveToken = true;
    ops.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        //ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
});

//register HttpClient to call other services
builder.Services.AddHttpClient();

//mapper
builder.Services.AddAutoMapper(typeof(AuthMapper));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
