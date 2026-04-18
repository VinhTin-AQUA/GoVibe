using Contracts.Models;
using Contracts.Places;
using FluentValidation;
using GoVibe.API.Controllers.Common;
using GoVibe.API.Messaging.RabbitMQ;
using GoVibe.API.Models;
using GoVibe.API.Models.Places;
using GoVibe.API.Services;
using GoVibe.API.Validators.Places;
using Microsoft.AspNetCore.Mvc;

namespace GoVibe.API.Controllers.Places
{
    public class AdminPlacesController : ControllerBaseApi
    {
        private readonly PlaceService _placeService;
        private readonly CategoryService _categoryService;
        private readonly RabbitMQService _rabbitMqService;
        private readonly AddPlaceRequestValidator _addPlaceRequestValidator;
        private readonly UpdatePlaceRequestValidator _updatePlaceRequestValidator;

        public AdminPlacesController(
            PlaceService placeService, 
            CategoryService categoryService,
            RabbitMQService rabbitMqService
        )
        {
            _placeService = placeService;
            _categoryService = categoryService;
            _rabbitMqService = rabbitMqService;
            _addPlaceRequestValidator = new AddPlaceRequestValidator();
            _updatePlaceRequestValidator = new UpdatePlaceRequestValidator();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddPlaceRequest request)
        {
            await _addPlaceRequestValidator.ValidateAndThrowAsync(request);
            var model = await _placeService.Add(request);
            
            var placeCategories = await _categoryService.GetCategoriesOfPlace(model.Id);
            
            var categoriesOfPlaceCreatedEvent = placeCategories.Select(x => new CategoryOfPlaceEvent
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            PlaceCreatedEvent placeCreatedEvent = new()
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Country = model.Country,
                TotalViews = model.TotalViews,
                TotalRating = model.TotalRating,
                TotalReviews = model.TotalReviews,
                Categories = categoriesOfPlaceCreatedEvent,
            };
            await _rabbitMqService.SendMessage<PlaceCreatedEvent>(placeCreatedEvent);
            return Ok(new ApiResponse<object>
            {
                Item = model
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string searchString = "", int pageIndex = 0, int pageSize = 20)
        {
            var r = await _placeService.GetAllPagination(searchString, pageIndex, pageSize);
            return Ok(new ApiResponse<object>
            {
                Item = r,
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdatePlaceRequest request)
        {
            await _updatePlaceRequestValidator.ValidateAndThrowAsync(request);
            var place = await _placeService.Update(request);
            return Ok(new ApiResponse<object>
            {
                Item = place,
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var place = await _placeService.Delete(id);
            return Ok(new ApiResponse<object>
            {
                Item = place,
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMany([FromBody] DeleteManyPlacesRequest request)
        {
            await _placeService.DeleteMany(request);
            return Ok(new ApiResponse<object>());
        }
    }
}
