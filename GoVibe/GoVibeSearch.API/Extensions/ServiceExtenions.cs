using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using GoVibeSearch.API.Configs;
using GoVibeSearch.API.Services;

namespace GoVibeSearch.API.Extensions
{
    public static class ServiceExtenions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddElasticService(services, configuration);

            AddServices(services, configuration);
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
    }
}
