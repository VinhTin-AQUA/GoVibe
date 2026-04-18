using Amazon;
using Amazon.S3;
using GoVibe.API.Configurations;
using GoVibe.API.Messaging.RabbitMQ;
using GoVibe.API.Services;
using MassTransit;

namespace GoVibe.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<CategoryService>();
            services.AddScoped<PlaceService>();
            services.AddScoped<ReviewService>();
            services.AddScoped<StatisticService>();
            
            services.AddSingleton<GarageService>();
            services.Configure<GarageConfig>(configuration.GetSection("Garage"));

            //services.AddSingleton<IAmazonS3>(sp =>
            //{
            //    var config = sp.GetRequiredService<IConfiguration>();

            //    var s3Config = new AmazonS3Config
            //    {
            //        ServiceURL = config["Garage:ServiceURL"], // http://localhost:3900
            //        ForcePathStyle = true,
            //        UseHttp = true,
            //        DisableHostPrefixInjection = true,

            //        AuthenticationRegion = "ap-southeast-1" // 
            //    };

            //    return new AmazonS3Client(
            //        config["Garage:AccessKey"],
            //        config["Garage:SecretKey"],
            //        s3Config
            //    );
            //});

            AddRabbitServices(services, configuration);
            services.AddScoped<RabbitMQService>();

            return services;
        }

        private static void AddRabbitServices(IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["RabbitMQ:Url"] ?? "";
            var userName = configuration["RabbitMQ:UserName"] ?? "";
            var password = configuration["RabbitMQ:Password"] ?? "";

            services.AddMassTransit(x =>
            {
                // Cấu hình sử dụng RabbitMQ
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(url, "/", h =>
                    {
                        h.Username(userName);
                        h.Password(password);
                    });
                });

            });
        }
    }
}
