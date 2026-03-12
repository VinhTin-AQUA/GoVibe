using AutoMapper;
using GoVibe.API.Profiles;

namespace GoVibe.API.Extensions
{
    public static class AutoMapperExtensions
    {
        private static readonly List<Type> types = 
        [
            typeof(CategoryProfile),
            typeof(AmenityProfile),
            typeof(PlaceProfile),
        ];

        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            foreach(var type in types)
            {
                services.AddAutoMapper((x) =>
                {
                    x.AddProfile(type);
                });
            }
           
            return services;
        }
    }
}
