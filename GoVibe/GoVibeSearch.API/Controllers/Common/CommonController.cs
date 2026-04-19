using GoVibeSearch.API.Models;
using GoVibeSearch.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoVibeSearch.API.Controllers.Common
{
    public class CommonController : ControllerBase
    {
        private readonly IPlaceSearchService _placeSearchService;

        public CommonController(IPlaceSearchService  placeSearchService)
        {
            _placeSearchService = placeSearchService;
        }
        
        [HttpGet("delete-all-record")]
        public async Task<IActionResult> DeleteAllRecordAsync()
        {
            var r = await _placeSearchService.DeleteAllRecordAsync();
            return Ok(new
            {
                success = r
            });
        }

        [HttpGet("init-index")]
        public async Task<IActionResult> InitIndex()
        {
            var place = new PlaceSearchModel()
            {
                Id = Guid.NewGuid(),
                Name = "Quán Cafe ABC",
                Address = "123 Nguyễn Huệ, Quận 1",
                Country = "Vietnam",
                TotalViews = 1000,
                TotalRating = 4.5,
                TotalReviews = 200,
                Categories = new List<CategorySearchModel>
                {
                    new CategorySearchModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Cafe"
                    },
                    new CategorySearchModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Dessert"
                    }
                },
                AverageRating = 0,
                Thumbnail = "",
                UpdatedAt = DateTime.Now,
                Status = "Open",
                Tags = ["Wonder"]
            };
            
            var all = await _placeSearchService.IndexAsync(place);
            
            return Ok(new
            {
                success = true
            });
        }

        [HttpGet("count-records")]
        public async Task<IActionResult> CountRecords()
        {
            var totalPlaces = await _placeSearchService.CountPlacesAsync();
            
            return Ok(new
            {
                totalPlaces
            });
        }
    }
}