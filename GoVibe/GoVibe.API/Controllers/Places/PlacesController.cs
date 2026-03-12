using GoVibe.API.Controllers.Common;
using GoVibe.API.Models;
using GoVibe.API.Models.Places;
using GoVibe.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoVibe.API.Controllers.Places
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBaseApi
    {
        private readonly PlaceService placeService;

        public PlacesController(PlaceService placeService)
        {
            this.placeService = placeService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddPlaceRequest request)
        {
            var r = await placeService.Add(request);
            return Ok(new ApiResponse<object>
            {
                Item = r
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageIndex = 0, int pageSize = 20)
        {
            var r = await placeService.GetAllPagination(pageIndex, pageSize);
            return Ok(new ApiResponse<object>
            {
                Item = r,
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdatePlaceRequest request)
        {
            var place = await placeService.Update(request);
            return Ok(new ApiResponse<object>
            {
                Item = place,
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var place = await placeService.Delete(id);
            return Ok(new ApiResponse<object>
            {
                Item = place,
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMany(DeleteManyPlacesRequest request)
        {
            await placeService.DeleteMany(request);
            return Ok(new ApiResponse<object>());
        }
    }
}
