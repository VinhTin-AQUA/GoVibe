using AutoMapper;
using GoVibe.API.Configurations;
using GoVibe.API.Exceptions;
using GoVibe.API.Models;
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
        private readonly IReviewCommandRepository _reviewCommandRepository;
        private readonly IReviewQueryRepository _reviewQueryRepository;
        private readonly IReviewImageCommandRepository _reviewImageCommandRepository;
        private readonly IReviewImageQueryRepository _reviewImageQueryRepository;
        private readonly IPlaceQueryRepository _placeQueryRepository;
        private readonly GarageService _garageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(
            IReviewCommandRepository reviewCommandRepository,
            IReviewQueryRepository reviewQueryRepository,
            IReviewImageCommandRepository reviewImageCommandRepository,
            IReviewImageQueryRepository reviewImageQueryRepository,
            IPlaceQueryRepository placeQueryRepository,
            GarageService  garageService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _reviewCommandRepository = reviewCommandRepository;
            _reviewQueryRepository = reviewQueryRepository;
            _reviewImageCommandRepository = reviewImageCommandRepository;
            _reviewImageQueryRepository = reviewImageQueryRepository;
            _placeQueryRepository = placeQueryRepository;
            _garageService = garageService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReviewModel> Add(AddReviewRequest request)
        {
            var place = await _placeQueryRepository.GetByIdAsync(Guid.Parse(request.PlaceId));
            if (place == null)
            {
                throw new NotFoundException("Place not found");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Review review = new()
                {
                    Id = Guid.NewGuid(),
                    PlaceId = Guid.Parse(request.PlaceId),
                    Rating = request.Rating,
                    Comment = request.Comment,
                };
                await _reviewCommandRepository.AddAsync(review);

                List<ReviewImage> reviewImages = [];
                foreach (var image in request.Images)
                {
                    string url = await _garageService.Upload(BucketPrefixKeyNames.ReviewImages, image);
                    ReviewImage reviewImage = new()
                    {
                        ImageUrl = url,
                        ReviewId = review.Id,
                    };
                    reviewImages.Add(reviewImage);
                }
                await _reviewImageCommandRepository.AddRangeAsync(reviewImages);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<ReviewModel>(review);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<Pagination<ReviewModel>> GetAllPagination(int pageIndex = 0, int pageSize = 20)
        {
            pageIndex = Math.Max(pageIndex, 1);   // >= 1
            pageSize = Math.Min(pageSize, 50);    // <= 50
            
            (List<Review> reviews, int total) = await _reviewQueryRepository.GetAllPagination(pageIndex, pageSize);
            return new Pagination<ReviewModel>
            {
                Items = _mapper.Map<List<ReviewModel>>(reviews),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = total,
                TotalPage = total / pageSize + 1
            };
        }

        public async Task<ReviewModel> EditReview(UpdateReviewModel model)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();  
                var review = await _reviewQueryRepository.GetByIdAsync(Guid.Parse(model.Id));
                if (review == null)
                {
                    throw new NotFoundException("Review not found");
                }

                review.Rating = model.Rating;
                review.Comment = model.Comment;

                // delete old images
                if (model.DeleteImageIds.Count > 0)
                {
                    var imageIds = model.DeleteImageIds.Select(x => Guid.Parse(x));
                    var oldImages = await _reviewImageQueryRepository.GetByIdsAsync(imageIds);

                    foreach (var img in oldImages)
                    {
                        await _garageService.DeleteAsync(img.ImageUrl);
                    }

                    await _reviewImageCommandRepository.DeleteRangeAsync(imageIds);
                }

                // add new images
                if (model.Images.Count > 0)
                {
                    List<ReviewImage> reviewImages = [];
                    foreach (var file in model.Images)
                    {
                        var key = await _garageService.Upload(BucketPrefixKeyNames.ReviewImages, file);
                        reviewImages.Add(new()
                        {
                            ImageUrl = key,
                            ReviewId = review.Id,
                        });
                    }

                    await _reviewImageCommandRepository.AddRangeAsync(reviewImages);
                }
                await _unitOfWork.CommitAsync();
                return _mapper.Map<ReviewModel>(review);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();  
                throw new Exception($"Error while editing review {model.Id}", ex);
            }
        }

        public async Task<ReviewModel> DeleteReview(string id)
        {
            var review = await _reviewQueryRepository.GetByIdAsync(Guid.Parse(id));
            if (review == null)
            {
                throw new NotFoundException("Review not found");
            }

            var reviewImages = await _reviewImageQueryRepository.GetListByReviewId(Guid.Parse(id));

            foreach (var reviewImage in reviewImages)
            {
                await _garageService.DeleteAsync(reviewImage.ImageUrl);
            }
            
            await _reviewCommandRepository.DeleteAsync(Guid.Parse(id));
            await _reviewCommandRepository.SaveChangesAsync();  
            return _mapper.Map<ReviewModel>(review);
        }
    }
}