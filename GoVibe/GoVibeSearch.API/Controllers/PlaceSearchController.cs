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
        
        [HttpGet("get-all-records")]
        public async Task<IActionResult> GetAllRecords()
        {
            var all = await _placeSearchService.GetAllAsync();
            
            return Ok(new { all });
        }
        
        [HttpPost("search")]
        public async Task<IActionResult> Search(PlaceSearchRequest request)
        {
            var all = await _placeSearchService.SearchPlacesAsync(request);
            var totalPlaces = await _placeSearchService.CountPlacesAsync();
            
            var r = new Pagination<PlaceSearchModel>
            {
                Items = all,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalCount = (int)totalPlaces,
                TotalPage = (int)totalPlaces / request.PageSize + 1
            };
            
            return Ok(new ApiResponse<object>
            {
                Item = r,
            });
        }
    }
}