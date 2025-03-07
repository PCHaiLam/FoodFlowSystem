using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities;
using FoodFlowSystem.Helpers;
using FoodFlowSystem.Mappers;
using FoodFlowSystem.Middlewares;
using FoodFlowSystem.Repositories;
using FoodFlowSystem.Repositories.Auth;
using FoodFlowSystem.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//HttpContextAccessor
builder.Services.AddHttpContextAccessor();

//Configuration
builder.Services.Configure<JwtSettingClass>(builder.Configuration.GetSection("JwtSettings"));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//JWT
builder.Services.AddScoped<JwtHelper>();

//Dependency Injection

//Repositories
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IAuthRepository, AuthRepository>();


//Services
builder.Services.AddScoped<IAuthService, AuthService>();

// Add DbContext
builder.Services.AddDbContext<MssqlDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlString")));

//Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(ops => {
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
