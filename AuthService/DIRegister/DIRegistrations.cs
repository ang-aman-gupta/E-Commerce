using AuthService.Services;

namespace AuthService.DIRegister
{
    public static class DIRegistrations
    {
        public static void RegisterDI(this IServiceCollection services)
        {
            services.AddScoped<RegisterUserService>();
            services.AddScoped<LoginService>();
            services.AddScoped<TokenService>();
        }

    }
}
