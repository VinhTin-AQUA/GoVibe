using Amazon.S3;
using GoVibe.API.Configurations;
using GoVibe.API.Services;

namespace GoVibe.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<CategoryService>();
            services.AddScoped<PlaceService>();
            services.AddScoped<ReviewService>();

            services.AddSingleton<GarageService>();
            
            services.Configure<GarageConfig>(configuration.GetSection("Garage"));
            
            services.AddSingleton<IAmazonS3>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();

                var s3Config = new AmazonS3Config
                {
                    ServiceURL = config["Garage:ServiceURL"],
                    ForcePathStyle = true,
                    UseHttp = true,

                    // 🔥 QUAN TRỌNG NHẤT
                    DisableHostPrefixInjection = true,
                    AuthenticationRegion = "garage",
                };

                return new AmazonS3Client(
                    config["Garage:AccessKey"],
                    config["Garage:SecretKey"],
                    s3Config
                );
            });
            
            return services;
        }
    }
}
