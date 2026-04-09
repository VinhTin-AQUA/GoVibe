using Amazon.Runtime.Internal;
using FluentValidation;
using GoVibe.API.Controllers.Common;
using GoVibe.API.Models;
using GoVibe.API.Models.Categories;
using GoVibe.API.Models.Places;
using GoVibe.API.Services;
using GoVibe.API.Validators.Places;
using GoVibe.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.API.Controllers.Places
{
    public class UserPlacesController : ControllerBaseApi
    {
        private readonly PlaceService _placeService;
        private readonly GarageService _garageService;
        private readonly AddPlaceRequestValidator _addPlaceRequestValidator;
        private readonly UpdatePlaceRequestValidator _updatePlaceRequestValidator;

        public UserPlacesController(PlaceService placeService, GarageService garageService)
        {
            _placeService = placeService;
            _garageService = garageService;
            _addPlaceRequestValidator = new AddPlaceRequestValidator();
            _updatePlaceRequestValidator = new UpdatePlaceRequestValidator();
        }

        [HttpGet("home")]
        public async Task<IActionResult> GetHome()
        {
            (
                List<PlaceModel> topRated,
                List<PlaceModel> mostViewed,
                List<PlaceModel> recent,
                List<PlaceModel> explore
            ) = await _placeService.GetHome();

            return Ok(new ApiResponse<object>
            {
                Item = new
                {
                    TopRated = topRated,
                    MostViewed = mostViewed,
                    Recent = recent,
                    Explore = explore
                }
            });
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] PlaceSearchRequest request)
        {
            var r = await _placeService.Search(request);
            return Ok(new ApiResponse<object>
            {
                Item = r
            });
        }
    }
}
