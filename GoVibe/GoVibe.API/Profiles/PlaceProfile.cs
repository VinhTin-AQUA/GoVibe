using AutoMapper;
using GoVibe.API.Models.Places;
using GoVibe.Domain.Entities;

namespace GoVibe.API.Profiles
{
    public class PlaceProfile : Profile
    {
        public PlaceProfile()
        {
            CreateMap<Place, PlaceModel>();
        }
    }
}
