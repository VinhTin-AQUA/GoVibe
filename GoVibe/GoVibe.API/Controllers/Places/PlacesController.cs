using GoVibe.API.Controllers.Common;
using GoVibe.API.Models;
using GoVibe.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoVibe.API.Controllers.Places
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesControllerr : ControllerBaseApi
    {
        private readonly PlaceService _placeService;

        public PlacesControllerr(PlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var r = await _placeService.Get(id);
            return Ok(new ApiResponse<object>
            {
                Item = r,
            });
        }
    }
}
