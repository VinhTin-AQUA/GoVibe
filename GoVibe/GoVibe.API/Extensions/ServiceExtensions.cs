using Amazon;
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
                    ServiceURL = config["Garage:ServiceURL"], // http://localhost:3900
                    ForcePathStyle = true,
                    UseHttp = true,
                    DisableHostPrefixInjection = true,

                    AuthenticationRegion = "ap-southeast-1" // 
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
