using AutoMapper;
using Contracts.Places;
using GoVibeSearch.API.Models;

namespace GoVibeSearch.API.Profiles
{
    public class PlaceSearchProfile : Profile
    {
        public PlaceSearchProfile()
        {
            CreateMap<PlaceCreatedEvent, PlaceSearchModel>();
        }
    }
}