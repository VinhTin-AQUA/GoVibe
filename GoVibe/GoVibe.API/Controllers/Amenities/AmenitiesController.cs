using GoVibe.API.Controllers.Common;
using GoVibe.API.Models;
using GoVibe.API.Models.Amenities;
using GoVibe.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoVibe.API.Controllers.Amenities
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenitiesController : ControllerBaseApi
    {
        private readonly AmenityService amenityService;

        public AmenitiesController(AmenityService amenityService)
        {
            this.amenityService = amenityService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAmenityRequest request)
        {
            var r = await amenityService.Add(request);
            return Ok(new ApiResponse<object>
            {
                Item = r
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageIndex = 0, int pageSize = 20)
        {
            var r = await amenityService.GetAllPagination(pageIndex, pageSize);
            return Ok(new ApiResponse<object>
            {
                Item = r,
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAmenityRequest request)
        {
            var amenity = await amenityService.Update(request);
            return Ok(new ApiResponse<object>
            {
                Item = amenity,
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var amenity = await amenityService.Delete(id);
            return Ok(new ApiResponse<object>
            {
                Item = amenity,
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMany(DeleteManyAmenitiesRequest request)
        {
            await amenityService.DeleteMany(request);
            return Ok(new ApiResponse<object>());
        }
    }
}
