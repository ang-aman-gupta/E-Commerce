using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using UserService.Data;
using UserService.Mapper;
using UserService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UserDbContext>(option =>
    
    option.UseSqlServer( builder.Configuration.GetConnectionString("UserDb")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:7271/"; // Replace with AuthService URL
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "AuthService", // Must match AuthService's issuer
            ValidAudience = "ECommerceAPI",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3DA01FCA-2457-426B-9FD3-D98F1D6FB88A-D930402B-7DBB-462D-BBDC-DE9BD9657E35-D930402B-7DBB-462D-BBDC-DE9BD9657E35")) // Same secret as AuthService
        };
    });

builder.Services.AddHostedService<KafkaConsumerService>(); // UserService Kafka Consumer
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<UserProfileService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
