using AutoMapper;
using GoVibe.API.Models.Places;
using GoVibe.Domain.Entities;

namespace GoVibe.API.Profiles
{
    public class PlaceImageProfile : Profile
    {
        public PlaceImageProfile()
        {
            CreateMap<PlaceImage, PlaceImageModel>();
        }
    }
}