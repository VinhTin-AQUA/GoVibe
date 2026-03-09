using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Amenities;
using GoVibe.Infrastructure.Repositories.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoVibe.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // services.AddDbContext<AppDbContext>(options =>
            // {
            //     options.UseNpgsql(configuration.GetConnectionString("PostgresConnectionStrings"));
            // });

            services.AddDbContextPool<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgresConnectionString")));
           
            services.AddPooledDbContextFactory<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgresConnectionString")));
            
            services.AddScoped<IAmenityCommandRepository, AmenityCommandRepository>();
            services.AddScoped<IAmenityQueryRepository, AmenityQueryRepository>();
            
            services.AddScoped<ICategoryCommandRepository, CategoryCommandRepository>();
            services.AddScoped<ICategoryQueryRepository, CategoryQueryRepository>();
            return services;
        }
    }
}