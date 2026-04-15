using AutoMapper;
using Bogus;
using GoVibe.API.Models;
using GoVibe.API.Services;
using GoVibe.Domain.Entities;
using GoVibe.Domain.Enums;
using GoVibe.Infrastructure.Repositories.Categories;
using GoVibe.Infrastructure.Repositories.PlaceImages;
using GoVibe.Infrastructure.Repositories.Places;
using GoVibe.Infrastructure.Repositories.Reviews;
using GoVibe.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

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
        private readonly IWebHostEnvironment _env;
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
            IWebHostEnvironment env,
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
            _env = env;
            _mapper = mapper;
        }

        [HttpGet("GenData")]
        public async Task<IActionResult> GenData()
        {
            Random random = new();

            //var faker = new Faker("vi");

            #region category

            var categoryFaker = new Faker<Category>()
                .RuleFor(x => x.Name, f => f.Commerce.Categories(1)[0] + "_" + f.UniqueIndex)
                .RuleFor(x => x.Description, f => f.Lorem.Sentence())
                .RuleFor(x => x.CreatedAt, f => f.Date.Between(
                    new DateTime(2010, 1, 1),
                    new DateTime(2030, 12, 31)
                ).ToUniversalTime())
                .RuleFor(x => x.UpdatedAt, f => f.Date.Between(
                    new DateTime(2010, 1, 1),
                    new DateTime(2030, 12, 31)
                ).ToUniversalTime())
                .RuleFor(x => x.Id, f => Guid.NewGuid());
            var categoryRequests = categoryFaker.Generate(20);
            var categoryIds = new List<Guid>();

            foreach (var category in categoryRequests)
            {
                await _categoryCommandRepository.AddAsync(category);
                categoryIds.Add(category.Id);
            }
            await _categoryCommandRepository.SaveChangesAsync();

            #endregion

            #region place

            var placeDescriptionSamplePath = Path.Combine(_env.ContentRootPath, "Uploads", "SampleData", "place_descriptions.json");
            var json = System.IO.File.ReadAllText(placeDescriptionSamplePath);
            var placeDescriptions = JsonSerializer.Deserialize<List<PlaceSeed>>(json);

            var tagsPool = new[]
            {
                "food", "coffee", "restaurant", "hotel",
                "travel", "family", "luxury", "cheap"
            };

            var placeFaker = new Faker<Place>()
                .RuleFor(x => x.Name, f => f.Company.CompanyName())
                .RuleFor(x => x.Description, f =>
                    f.PickRandom(placeDescriptions).Description
                )
                .RuleFor(x => x.Address, f => f.Address.FullAddress())
                .RuleFor(x => x.Country, f => f.Address.Country())
                .RuleFor(x => x.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(x => x.Website, f => f.Internet.Url())
                .RuleFor(x => x.OpeningHours, f => "08:00 - 22:00")
                .RuleFor(x => x.TotalReviews, f => f.Random.Int(100, 1000))
                .RuleFor(x => x.TotalRating, (f, x) =>
                {
                    int total = 0;
                    for (int i = 0; i < x.TotalReviews; i++)
                    {
                        total += f.Random.Int(1, 5);
                    }
                    return total;
                })
                .RuleFor(x => x.TotalViews, f => f.Random.Int(100, 10000))
                .RuleFor(x => x.Status, f => EPlaceStatus.Open)
                .RuleFor(x => x.CreatedAt, f => f.Date.Between(
                    new DateTime(2010, 1, 1),
                    new DateTime(2030, 12, 31)
                ).ToUniversalTime())
                .RuleFor(x => x.UpdatedAt, f => f.Date.Between(
                    new DateTime(2010, 1, 1),
                    new DateTime(2030, 12, 31)
                ).ToUniversalTime())
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

            #endregion

            #region placeCategory

            var dateFaker = new Faker();
            foreach (var placeId in placeIds)
            {
                DateTime randomDate = dateFaker.Date.Between(
                    new DateTime(2010, 1, 1),
                    new DateTime(2030, 12, 31)
                ).ToUniversalTime();

                var placeCategory = new PlaceCategory()
                {
                    PlaceId = placeId,
                    CategoryId = categoryIds[random.Next(0, categoryIds.Count)],
                    CreatedAt = randomDate,
                    UpdatedAt = randomDate,
                };

                await _placeCategoryCommandRepository.AddAsync(placeCategory);
            }
            await _placeCategoryCommandRepository.SaveChangesAsync();

            #endregion

            #region Review

            var reviewFaker = new Faker<Review>()
                .RuleFor(x => x.PlaceId, f => f.PickRandom(placeIds))
                .RuleFor(x => x.Rating, f => f.Random.Int(1, 5))
                .RuleFor(x => x.CreatedAt, f => f.Date.Between(
                    new DateTime(2010, 1, 1),
                    new DateTime(2030, 12, 31)
                ).ToUniversalTime())
                .RuleFor(x => x.UpdatedAt, f => f.Date.Between(
                    new DateTime(2010, 1, 1),
                    new DateTime(2030, 12, 31)
                ).ToUniversalTime())
                .RuleFor(x => x.Comment, f => f.Lorem.Sentence());

            var reviewRequests = reviewFaker.Generate(1000);
            foreach (var review in reviewRequests)
            {
                await _reviewCommandRepository.AddAsync(review);
            }
            await _reviewCommandRepository.SaveChangesAsync();

            #endregion


            // review image

            #region place image

            var placeImages = new List<PlaceImage>();
            int width = 1200;
            int height = 800;

            foreach (var placeId in placeIds)
            {
                int imageCount = random.Next(3, 6); // 3 → 5

                for (int i = 0; i < imageCount; i++)
                {
                    int seed = random.Next(1, 1000000);

                    string url = $"https://picsum.photos/seed/{seed}/{width}/{height}";

                    placeImages.Add(new PlaceImage
                    {
                        PlaceId = placeId,
                        ImageUrl = url
                    });
                }
            }
            await _placeImageCommandRepository.AddRangeAsync(placeImages);
            await _placeImageCommandRepository.SaveChangesAsync();

            #endregion

            return Ok(new
            {
                Message = "Success"
            });
        }

        [HttpGet("RemoveAllData")]
        public async Task<IActionResult> RemoveAllData()
        {
            var allPlaceCategories = await _placeCategoryQueryRepository.GetAllAsync();
            await _placeCategoryCommandRepository.DeleteRangeAsync(allPlaceCategories);
            await _placeCategoryCommandRepository.SaveChangesAsync();

            var allReviews = await _reviewQueryRepository.GetAllAsync();
            await _reviewCommandRepository.DeleteRangeAsync(allReviews);
            await _reviewCommandRepository.SaveChangesAsync();

            var allPaces = await _placeQueryRepository.GetAllAsync();
            await _placeCommandRepository.DeleteRangeAsync(allPaces);
            await _placeCommandRepository.SaveChangesAsync();

            var allCates = await _categoryQueryRepository.GetAllAsync();
            await _categoryCommandRepository.DeleteRangeAsync(allCates);
            await _categoryCommandRepository.SaveChangesAsync();

            var placeImages = await _placeImageQueryRepository.GetAllAsync();
            await _placeImageCommandRepository.DeleteRangeAsync(placeImages);
            await _placeImageCommandRepository.SaveChangesAsync();

            return Ok(new
            {
                Message = "Success"
            });
        }
        
        [HttpGet("country-options")]
        public IActionResult GetCountryOptions()
        {
            var countryOptions = new List<Options<string, string?>>
            {
                new() { Label = "None", Value = null },
                new() { Label = "Vietnam", Value = "vietnam" },
                new() { Label = "United States", Value = "usa" },
                new() { Label = "Japan", Value = "japan" },
                new() { Label = "South Korea", Value = "korea" },
                new() { Label = "China", Value = "china" },
                new() { Label = "United Kingdom", Value = "uk" },
                new() { Label = "France", Value = "france" },
                new() { Label = "Germany", Value = "germany" },
                new() { Label = "Australia", Value = "australia" },
                new() { Label = "Canada", Value = "canada" }
            };

            return Ok(new ApiResponse<object>
            {
                Item = countryOptions
            });
        }
    }

    public class PlaceSeed
    {
        public string Description { get; set; } = "";
    }
}
