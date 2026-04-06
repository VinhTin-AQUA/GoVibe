using AutoMapper;
using GoVibe.API.Models.Reviews;
using GoVibe.Domain.Entities;

namespace GoVibe.API.Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewModel>();
            CreateMap<ReviewImage, ReviewImageModel>();
            
        }
    }
}