using AutoMapper;
using GoVibe.API.Exceptions;
using GoVibe.API.Models.Reviews;
using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Repositories.Places;
using GoVibe.Infrastructure.Repositories.ReviewImages;
using GoVibe.Infrastructure.Repositories.Reviews;
using GoVibe.Infrastructure.UnitOfWork;

namespace GoVibe.API.Services
{
    public class ReviewService
    {
        private readonly IReviewCommandRepository reviewCommandRepository;
        private readonly IReviewImageCommandRepository reviewImageCommandRepository;
        private readonly IPlaceQueryRepository placeQueryRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ReviewService(
            IReviewCommandRepository reviewCommandRepository,
            IReviewImageCommandRepository reviewImageCommandRepository,
            IPlaceQueryRepository placeQueryRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.reviewCommandRepository = reviewCommandRepository;
            this.reviewImageCommandRepository = reviewImageCommandRepository;
            this.placeQueryRepository = placeQueryRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ReviewModel> Add(AddReviewRequest request)
        {
            var place = await placeQueryRepository.GetByIdAsync(Guid.Parse(request.PlaceId));
            if (place == null)
            {
                throw new NotFoundException("Place not found");
            }

            try
            {
                await unitOfWork.BeginTransactionAsync();
                Review review = new()
                {
                    Id = Guid.NewGuid(),
                    PlaceId = Guid.Parse(request.PlaceId),
                    Rating = request.Rating,
                    Comment = request.Comment,
                };
                await reviewCommandRepository.AddAsync(review);

                List<ReviewImage> reviewImages = [];
                foreach (var image in request.Images)
                {
                    string url = "";
                    ReviewImage reviewImage = new()
                    {
                        ImageUrl = url,
                        ReviewId = review.Id,
                    };
                    reviewImages.Add(reviewImage);
                }
                await reviewImageCommandRepository.AddRangeAsync(reviewImages);
                await unitOfWork.CommitAsync();
                return mapper.Map<ReviewModel>(review);
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task GetAllPagination(int pageIndex = 0, int pageSize = 20)
        {
            pageIndex = Math.Max(pageIndex, 1);   // >= 1
            pageSize = Math.Min(pageSize, 50);    // <= 50
        }

        public async Task EditReview()
        {

        }

        public async Task RemoveReview()
        {

        }
    }
}