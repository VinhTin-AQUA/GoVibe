using AutoMapper;
using GoVibe.API.Models.Amenities;
using GoVibe.Domain.Entities;

namespace GoVibe.API.Profiles
{
    public class AmenityProfile : Profile
    {
        public AmenityProfile()
        {
            CreateMap<Amenity, AmenityModel>();
        }
    }
}
