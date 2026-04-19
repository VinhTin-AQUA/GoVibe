using AutoMapper;
using Contracts.Places;
using GoVibe.API.Models.Places;
using GoVibe.Domain.Entities;

namespace GoVibe.API.Profiles
{
    public class PlaceProfile : Profile
    {
        public PlaceProfile()
        {
            CreateMap<Place, PlaceModel>()
                .ForMember(x => x.AverageRating, y => y.MapFrom(z => z.TotalRating / z.TotalReviews))
                .ForMember(x => x.Categories, y => y.MapFrom(z => z.PlaceCategories.Select(x => x.Category).ToList()));

            CreateMap<Place, PlaceDetailsModel>()
                .ForMember(x => x.Reviews, y => y.MapFrom(z => z.Reviews.OrderByDescending(x => x.UpdatedAt).ToList()))
                .ForMember(x => x.Categories, y => y.MapFrom(z => z.PlaceCategories.Select(x =>x.Category).ToList()));
            
            CreateMap<Place, PlaceCreatedEvent>()
                .ForMember(x => x.AverageRating, y => y.MapFrom(z => z.TotalRating / z.TotalReviews))
                .ForMember(x => x.Categories, y => y.MapFrom(z => z.PlaceCategories.Select(x => x.Category).ToList()));
        }
    }
}
