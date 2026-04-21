using GoVibeAuth.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoVibeAuth.Infrastructure.Extensions
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

            return services;
        }
    }
}
