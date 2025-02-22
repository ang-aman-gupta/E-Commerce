using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using UserService.Data;
using UserService.Mapper;
using UserService.Service;

IdentityModelEventSource.ShowPII = true;

var builder = WebApplication.CreateBuilder(args);
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:Key"]));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
  options.RequireHttpsMetadata = false; // Set to true in production
  options.SaveToken = true;
  options.Authority = "https://localhost:7271/";
  options.TokenValidationParameters = new TokenValidationParameters
  {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = key, // Ensure this matches AuthService
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidIssuer = builder.Configuration["Token:Issuer"], // Ensure this matches AuthService
      ValidAudience = builder.Configuration["Token:Audience"], // Ensure this matches AuthService
      ClockSkew = TimeSpan.Zero // Prevents token delay issues
  };
  options.Events = new JwtBearerEvents
  {
     OnAuthenticationFailed = context =>
     {
         Console.WriteLine($"🔴 Authentication Failed: {context.Exception.Message}");
         if (context.Exception.InnerException != null)
         {
             Console.WriteLine($"🔴 Inner Exception: {context.Exception.InnerException.Message}");
         }
         return Task.CompletedTask;
     },
     OnTokenValidated = context =>
     {
         Console.WriteLine("✅ Token Validated Successfully!");
         return Task.CompletedTask;
     }
  };

}
);

builder.Services.AddAuthorization();

builder.Services.AddDbContext<UserDbContext>(option =>

    option.UseSqlServer(builder.Configuration.GetConnectionString("UserDb")));
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<UserProfileService>();
builder.Services.AddSingleton<IConsumer<Ignore, string>>(sp =>
{
    var config = new ConsumerConfig
    {
        BootstrapServers = "localhost:9092",
        GroupId = "user-service-group",
        AutoOffsetReset = AutoOffsetReset.Earliest,
        EnableAutoCommit = false // Ensure manual commit
    };

    return new ConsumerBuilder<Ignore, string>(config).Build();
});
builder.Services.AddHostedService<KafkaConsumerService>();// UserService Kafka Consumer
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
app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"];
    Console.WriteLine($"🔍 Incoming Token: {token}");
    await next();
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
