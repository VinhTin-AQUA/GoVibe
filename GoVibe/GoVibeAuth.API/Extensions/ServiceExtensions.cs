using GoVibeAuth.API.Configs;
using GoVibeAuth.API.Services;

namespace GoVibeAuth.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<GoogleSettings>(configuration.GetSection("GoogleSettings").Get<GoogleSettings>()!);
            services.AddSingleton<GoogleService>();
            services.AddScoped<JwtService>();
            services.AddScoped<AuthService>();
            
            return services;
        }
    }
}