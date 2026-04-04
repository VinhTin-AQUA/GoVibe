using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Categories;
using GoVibe.Infrastructure.Repositories.PlaceImages;
using GoVibe.Infrastructure.Repositories.Places;
using GoVibe.Infrastructure.Repositories.ReviewImages;
using GoVibe.Infrastructure.Repositories.Reviews;
using GoVibe.Infrastructure.UnitOfWork;
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
            
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            
            services.AddScoped<ICategoryCommandRepository, CategoryCommandRepository>();
            services.AddScoped<ICategoryQueryRepository, CategoryQueryRepository>();
                
            services.AddScoped<IPlaceCommandRepository, PlaceCommandRepository>();
            services.AddScoped<IPlaceQueryRepository, PlaceQueryRepository>();
            
            services.AddScoped<IPlaceImageCommandRepository, PlaceImageCommandRepository>();
            services.AddScoped<IPlaceImageQueryRepository, PlaceImageQueryRepository>();
            
            services.AddScoped<IReviewCommandRepository, ReviewCommandRepository>();
            services.AddScoped<IReviewQueryRepository, ReviewQueryRepository>();
            
            services.AddScoped<IReviewImageCommandRepository, ReviewImageCommandRepository>();
            services.AddScoped<IReviewImageQueryRepository, ReviewImageQueryRepository>();

            services.AddScoped<IPlaceCategoryQueryRepository, PlaceCategoryQueryRepository>();
            services.AddScoped<IPlaceCategoryCommandRepository, PlaceCategoryCommandRepository>();
            return services;
        }
    }
}