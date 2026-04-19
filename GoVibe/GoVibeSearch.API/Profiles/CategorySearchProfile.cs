using AutoMapper;
using Contracts.Models;
using GoVibeSearch.API.Models;

namespace GoVibeSearch.API.Profiles
{
    public class CategorySearchProfile : Profile
    {
        public CategorySearchProfile()
        {
            CreateMap<CategoryOfPlaceEvent, CategorySearchModel>();
        }
    }
}