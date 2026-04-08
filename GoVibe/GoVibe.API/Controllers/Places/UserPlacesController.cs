using FluentValidation;
using GoVibe.API.Controllers.Common;
using GoVibe.API.Models;
using GoVibe.API.Services;
using GoVibe.API.Validators.Places;
using Microsoft.AspNetCore.Mvc;

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
        
        
    }
}
