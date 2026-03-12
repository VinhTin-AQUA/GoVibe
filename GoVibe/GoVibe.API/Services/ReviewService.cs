using AutoMapper;
using GoVibe.API.Models.Reviews;
using GoVibe.Infrastructure.Repositories.ReviewImages;
using GoVibe.Infrastructure.Repositories.Reviews;

namespace GoVibe.API.Services
{
    public class ReviewService
    {
        private readonly IReviewCommandRepository reviewCommandRepository;
        private readonly IReviewQueryRepository reviewQueryRepository;
        private readonly IReviewImageCommandRepository reviewImageCommandRepository;
        private readonly IReviewImageQueryRepository reviewImageQueryRepository;
        private readonly IMapper mapper;

        public ReviewService(
            IReviewCommandRepository reviewCommandRepository,
            IReviewQueryRepository reviewQueryRepository,
            IReviewImageCommandRepository reviewImageCommandRepository,
            IReviewImageQueryRepository reviewImageQueryRepository,
            IMapper mapper)
        {
            this.reviewCommandRepository = reviewCommandRepository;
            this.reviewQueryRepository = reviewQueryRepository;
            this.reviewImageCommandRepository = reviewImageCommandRepository;
            this.reviewImageQueryRepository = reviewImageQueryRepository;
            this.mapper = mapper;
        }
        
        // add review to place
        // public async Task<ReviewModel> AddReview()
        // {
        //     
        // }
        
        // edit review
        
        // delete review
    }
}