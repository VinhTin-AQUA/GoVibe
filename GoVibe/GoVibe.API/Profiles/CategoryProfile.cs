using AutoMapper;
using GoVibe.API.Models;
using GoVibe.API.Models.Categories;
using GoVibe.Domain.Entities;

namespace GoVibe.API.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryModel>();
            CreateMap<Category, Options<string, string>>()
                .ForMember(x =>x.Label, y => y.MapFrom(z => z.Name))
                .ForMember(x =>x.Value, y => y.MapFrom(z => z.Id.ToString()));
        }
    }
}
