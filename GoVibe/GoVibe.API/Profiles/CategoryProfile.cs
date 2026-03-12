using AutoMapper;
using GoVibe.API.Models.Categories;
using GoVibe.Domain.Entities;

namespace GoVibe.API.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryModel>();
        }
    }
}
