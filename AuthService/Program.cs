using AuthService.DBContext;
using AuthService.DIRegister;
using AuthService.Models;
using AuthService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterDI();
builder.Services.AddDbContext<AuthServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerDb"));
});
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<AuthServiceDbContext>()
    .AddDefaultTokenProviders();

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:Key"]));
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(opt =>
{
    opt.SaveToken = true;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,

    };
});
builder.Services.AddHostedService<OutBoxProcesser>();  // AuthService Outbox Processor
builder.Services.AddSingleton<KafkaProducerService>();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;
var identityDbContext = serviceProvider.GetRequiredService<AuthServiceDbContext>();
var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

try
{
    await identityDbContext.Database.MigrateAsync();
    await RoleService.SeedIniData(userManager, roleManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred while tying to apply migration.");
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthServiceAPI");
        

    });
    app.MapScalarApiReference();

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
