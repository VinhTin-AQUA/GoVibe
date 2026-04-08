using Bogus;
using GoVibe.API.Models.Categories;
using GoVibe.API.Models.Places;
using GoVibe.API.Models.Reviews;
using GoVibe.API.Services;
using GoVibe.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GoVibe.API.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly PlaceService _placeService;
        private readonly ReviewService _reviewService;

        public CommonController(CategoryService categoryService, 
            PlaceService placeService,
            ReviewService reviewService
            )
        {
            _categoryService = categoryService;
            _placeService = placeService;
            _reviewService = reviewService;
        }

        [HttpGet("GenData")]
        public async Task<IActionResult> GenData()
        {
            var faker = new Faker("vi");
            var categoryFaker = new Faker<AddCategoryRequest>()
                .RuleFor(x => x.Name, f => f.Commerce.Categories(1)[0] + "_" + f.UniqueIndex)
                .RuleFor(x => x.Description, f => f.Lorem.Sentence());

            var categoryRequests = categoryFaker.Generate(20);

            var categoryIds = new List<Guid>();

            foreach (var category in categoryRequests)
            {
                var result = await _categoryService.Add(category);
                categoryIds.Add(Guid.Parse(result.Id));
            }

            var tagsPool = new[]
            {
                "food", "coffee", "restaurant", "hotel",
                "travel", "family", "luxury", "cheap"
            };

            var placeFaker = new Faker<AddPlaceRequest>()
                .RuleFor(x => x.Name, f => f.Company.CompanyName())
                .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
                .RuleFor(x => x.Address, f => f.Address.FullAddress())
                .RuleFor(x => x.Country, f => f.Address.Country())
                .RuleFor(x => x.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(x => x.Website, f => f.Internet.Url())
                .RuleFor(x => x.OpeningHours, f => "08:00 - 22:00")
                .RuleFor(x => x.Status, f => EPlaceStatus.Open)
                .RuleFor(x => x.Images, f => new List<IFormFile>())
                .RuleFor(x => x.Tags, f =>
                    f.PickRandom(tagsPool, f.Random.Int(1, 5)).ToList())

                .RuleFor(x => x.CategoryIds, f =>
                    f.PickRandom(categoryIds, f.Random.Int(1, 3)).ToList()
                );

            var placeRequests = placeFaker.Generate(500);

            var placeIds = new List<Guid>();

            foreach (var place in placeRequests)
            {
                var result = await _placeService.Add(place);
                placeIds.Add(Guid.Parse(result.Id));
            }

            var reviewFaker = new Faker<AddReviewRequest>()
                .RuleFor(x => x.PlaceId, f => f.PickRandom(placeIds).ToString())
                .RuleFor(x => x.Rating, f => f.Random.Int(1, 5))
                .RuleFor(x => x.Comment, f => f.Lorem.Sentence())
                .RuleFor(x => x.Images, f => new List<IFormFile>());

            var reviewRequests = reviewFaker.Generate(1000);
            foreach (var review in reviewRequests)
            {
                await _reviewService.Add(review);
            }

            return Ok(new
            {
                Message = "Success"
            });
        }

        [HttpGet("RemoveAllData")]
        public async Task<IActionResult> RemoveAllData()
        {
            await _reviewService.RemoveAllData();
            await _placeService.RemoveAllData();
            await _categoryService.RemoveAllData();

            return Ok(new
            {
                Message = "Success"
            });
        }
    }
}
