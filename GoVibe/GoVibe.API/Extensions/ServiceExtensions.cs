using GoVibe.API.Services;

namespace GoVibe.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<CategoryService>();
            services.AddScoped<PlaceService>();
            services.AddScoped<ReviewService>();
            
            return services;
        }
    }
}
