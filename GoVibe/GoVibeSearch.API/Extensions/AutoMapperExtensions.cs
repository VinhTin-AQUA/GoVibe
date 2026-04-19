using GoVibeSearch.API.Profiles;

namespace GoVibeSearch.API.Extensions
{
    public static class AutoMapperExtensions
    {
        private static readonly List<Type> types = 
        [
            typeof(CategorySearchProfile),
            typeof(PlaceSearchProfile),
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