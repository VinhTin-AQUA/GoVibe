using GoVibeSearch.API.Controllers.Common;
using GoVibeSearch.API.Models;
using GoVibeSearch.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoVibeSearch.API.Controllers
{
    public class PlaceSearchController : ControllerBaseApi
    {
        private readonly IPlaceSearchService _placeSearchService;

        public PlaceSearchController(IPlaceSearchService  placeSearchService)
        {
            _placeSearchService = placeSearchService;
        }
        
        [HttpGet("GetIndexName")]
        public IActionResult GetIndexName()
        {
            var index = _placeSearchService.GetIndexName();
            
           return Ok(new { index });
        }
        
        [HttpGet("add-record")]
        public async Task<IActionResult> AddRecord()
        {
            var place = new PlaceSearchModel
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
                }
            };
            
            var all = await _placeSearchService.IndexAsync(place);
            
            return Ok(new { all });
        }
        
        [HttpGet("get-all-records")]
        public async Task<IActionResult> GetAllRecords()
        {
            var all = await _placeSearchService.GetAllAsync();
            
            return Ok(new { all });
        }
    }
}