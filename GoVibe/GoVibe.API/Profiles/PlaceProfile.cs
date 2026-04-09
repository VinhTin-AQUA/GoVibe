using AutoMapper;
using GoVibe.API.Models.Places;
using GoVibe.Domain.Entities;

namespace GoVibe.API.Profiles
{
    public class PlaceProfile : Profile
    {
        public PlaceProfile()
        {
            CreateMap<Place, PlaceModel>()
                .ForMember(x => x.Categories, y => y.MapFrom(z => z.PlaceCategories.Select(x => x.Category).ToList()));

            CreateMap<Place, PlaceDetailsModel>()
                .ForMember(x => x.Categories, y => y.MapFrom(z => z.PlaceCategories.Select(x =>x.Category).ToList()));
        }
    }
}
