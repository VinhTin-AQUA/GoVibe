using AutoMapper;
using Bogus;
using GoVibe.API.Models.Categories;
using GoVibe.API.Models.Places;
using GoVibe.API.Models.Reviews;
using GoVibe.API.Services;
using GoVibe.Domain.Entities;
using GoVibe.Domain.Enums;
using GoVibe.Infrastructure.Repositories.Categories;
using GoVibe.Infrastructure.Repositories.PlaceImages;
using GoVibe.Infrastructure.Repositories.Places;
using GoVibe.Infrastructure.Repositories.Reviews;
using GoVibe.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace GoVibe.API.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IPlaceQueryRepository _placeQueryRepository;
        private readonly IPlaceCommandRepository _placeCommandRepository;
        private readonly IPlaceImageCommandRepository _placeImageCommandRepository;
        private readonly IPlaceImageQueryRepository _placeImageQueryRepository;
        private readonly IPlaceCategoryQueryRepository _placeCategoryQueryRepository;
        private readonly IPlaceCategoryCommandRepository _placeCategoryCommandRepository;
        private readonly ICategoryCommandRepository _categoryCommandRepository;
        private readonly ICategoryQueryRepository _categoryQueryRepository;
        private readonly IReviewCommandRepository _reviewCommandRepository;
        private readonly IReviewQueryRepository _reviewQueryRepository;
        private readonly GarageService _garageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommonController(
            IPlaceQueryRepository placeQueryRepository,
            IPlaceCommandRepository placeCommandRepository,
            IPlaceImageCommandRepository placeImageCommandRepository,
            IPlaceImageQueryRepository placeImageQueryRepository,
            IPlaceCategoryQueryRepository placeCategoryQueryRepository,
            IPlaceCategoryCommandRepository placeCategoryCommandRepository,
            ICategoryCommandRepository categoryCommandRepository,
            ICategoryQueryRepository categoryQueryRepository,
            IReviewCommandRepository reviewCommandRepository,
            IReviewQueryRepository reviewQueryRepository,
            GarageService garageService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _placeQueryRepository = placeQueryRepository;
            _placeCommandRepository = placeCommandRepository;
            _placeImageCommandRepository = placeImageCommandRepository;
            _placeImageQueryRepository = placeImageQueryRepository;
            _placeCategoryQueryRepository = placeCategoryQueryRepository;
            _placeCategoryCommandRepository = placeCategoryCommandRepository;
            _categoryCommandRepository = categoryCommandRepository;
            _categoryQueryRepository = categoryQueryRepository;
            _reviewCommandRepository = reviewCommandRepository;
            _reviewQueryRepository = reviewQueryRepository;
            _garageService = garageService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GenData")]
        public async Task<IActionResult> GenData()
        {
            var faker = new Faker("vi");
            var categoryFaker = new Faker<Category>()
                .RuleFor(x => x.Name, f => f.Commerce.Categories(1)[0] + "_" + f.UniqueIndex)
                .RuleFor(x => x.Description, f => f.Lorem.Sentence())
                .RuleFor(x => x.Id, f => Guid.NewGuid());
            var categoryRequests = categoryFaker.Generate(20);
            var categoryIds = new List<Guid>();

            foreach (var category in categoryRequests)
            {
                await _categoryCommandRepository.AddAsync(category);
                categoryIds.Add(category.Id);
            }
            await _categoryCommandRepository.SaveChangesAsync();

            var tagsPool = new[]
            {
                "food", "coffee", "restaurant", "hotel",
                "travel", "family", "luxury", "cheap"
            };

            var placeFaker = new Faker<Place>()
                .RuleFor(x => x.Name, f => f.Company.CompanyName())
                .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
                .RuleFor(x => x.Address, f => f.Address.FullAddress())
                .RuleFor(x => x.Country, f => f.Address.Country())
                .RuleFor(x => x.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(x => x.Website, f => f.Internet.Url())
                .RuleFor(x => x.OpeningHours, f => "08:00 - 22:00")
                .RuleFor(x => x.TotalRating, f => f.Random.Int(100, 10000))
                .RuleFor(x => x.TotalReviews, f => f.Random.Int(100, 10000))
                .RuleFor(x => x.TotalViews, f => f.Random.Int(100, 10000))
                .RuleFor(x => x.Status, f => EPlaceStatus.Open)
                .RuleFor(x => x.Tags, f => f.PickRandom(tagsPool, f.Random.Int(1, 5)).ToList())
                .RuleFor(x => x.Thumbnail, f => "https://upload.wikimedia.org/wikipedia/commons/3/3f/JPEG_example_flower.jpg");

            var placeRequests = placeFaker.Generate(500);
            var placeIds = new List<Guid>();
            foreach (var place in placeRequests)
            {
                await _placeCommandRepository.AddAsync(place);
                placeIds.Add(place.Id);
            }
            await _placeCommandRepository.SaveChangesAsync();


            var reviewFaker = new Faker<Review>()
                .RuleFor(x => x.PlaceId, f => f.PickRandom(placeIds))
                .RuleFor(x => x.Rating, f => f.Random.Int(1, 5))
                .RuleFor(x => x.Comment, f => f.Lorem.Sentence());

            var reviewRequests = reviewFaker.Generate(1000);
            foreach (var review in reviewRequests)
            {
                await _reviewCommandRepository.AddAsync(review);
            }
            await _reviewCommandRepository.SaveChangesAsync();

            return Ok(new
            {
                Message = "Success"
            });
        }

        [HttpGet("RemoveAllData")]
        public async Task<IActionResult> RemoveAllData()
        {
            var allReviews = await _reviewQueryRepository.GetAllAsync();
            await _reviewCommandRepository.DeleteRangeAsync(allReviews);
            await _reviewCommandRepository.SaveChangesAsync();

            var allPaces = await _placeQueryRepository.GetAllAsync();
            await _placeCommandRepository.DeleteRangeAsync(allPaces);
            await _placeCommandRepository.SaveChangesAsync();

            var allCates = await _categoryQueryRepository.GetAllAsync();
            await _categoryCommandRepository.DeleteRangeAsync(allCates);
            await _categoryCommandRepository.SaveChangesAsync();


            return Ok(new
            {
                Message = "Success"
            });
        }
    }
}
