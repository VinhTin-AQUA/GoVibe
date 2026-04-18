using Contracts.Configs;
using Contracts.Places;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using GoVibeSearch.API.Configs;
using GoVibeSearch.API.Messaging.RabbitMQ.Consumers;
using GoVibeSearch.API.Services;
using MassTransit;

namespace GoVibeSearch.API.Extensions
{
    public static class ServiceExtenions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddElasticService(services, configuration);
            AddServices(services, configuration);
            AddRabbitMQ(services, configuration);
        }

        private static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IElasticService<>), typeof(ElasticService<>));
            services.AddScoped<IPlaceSearchService, PlaceSearchService>();
            
            services.Configure<ElasticIndexes>(configuration.GetSection("Elasticsearch:ElasticIndexes"));
        }

        private static void AddElasticService(IServiceCollection services, IConfiguration configuration)
        {
            var elasticUrl = configuration["Elasticsearch:Url"] ?? "";
            var username = configuration["Elasticsearch:Username"] ?? "";
            var password = configuration["Elasticsearch:Password"] ?? "";
            var defaultIndex = configuration["Elasticsearch:DefaultIndex"] ?? "";

            var settings = new ElasticsearchClientSettings(new Uri(elasticUrl))
                .Authentication(new BasicAuthentication(username, password))
                .DefaultIndex(defaultIndex);
      
            services.AddSingleton<ElasticsearchClient>(new ElasticsearchClient(settings));
        }

        private static void AddRabbitMQ(IServiceCollection services, IConfiguration configuration)
        {
            List<(Type, string)> consumers = 
            [
                (typeof(PlaceCreatedConsumer), PlaceQueueNames.PlaceCreated)
            ];
            
            services.AddMassTransit(x =>
            {
                var url = configuration["RabbitMQ:Url"] ?? "";
                var userName = configuration["RabbitMQ:UserName"] ?? "";
                var password = configuration["RabbitMQ:Password"] ?? "";

                foreach (var (consumerType, queue) in consumers)
                {
                    x.AddConsumer(consumerType);
                }
   
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(url, "/", h =>
                    {
                        h.Username(userName);
                        h.Password(password);
                    });
                    
                    foreach (var (consumerType, queue) in consumers)
                    {
                        cfg.ReceiveEndpoint(queue, e =>
                        {
                            e.ConfigureConsumer(context, consumerType);
                        });
                    }
                });
            });
        }
    }
}
