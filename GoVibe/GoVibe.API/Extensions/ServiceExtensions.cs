using GoVibe.API.Services;

namespace GoVibe.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<CategoryService>();
            services.AddScoped<AmenityService>();
            services.AddScoped<PlaceService>();

            return services;
        }
    }
}
