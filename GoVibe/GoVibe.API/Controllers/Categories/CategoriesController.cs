using FluentValidation;
using GoVibe.API.Controllers.Common;
using GoVibe.API.Models;
using GoVibe.API.Models.Categories;
using GoVibe.API.Services;
using GoVibe.API.Validators.Categories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GoVibe.API.Controllers.Categories
{
    public class CategoriesController : ControllerBaseApi
    {
        private readonly CategoryService _categoryService;
        private readonly AddCategoryRequestValidator _addCategoryRequestValidator;
        private readonly UpdateCategoryRequestValidator _updateCategoryRequestValidator;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
            _addCategoryRequestValidator = new AddCategoryRequestValidator();
            _updateCategoryRequestValidator = new UpdateCategoryRequestValidator();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryRequest request)
        {
            await _addCategoryRequestValidator.ValidateAndThrowAsync(request);
            var r = await _categoryService.Add(request);
            return Ok(new ApiResponse<object>
            {
                Item = r
            });
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var r = await _categoryService.GetById(id);
            return Ok(new ApiResponse<object>
            {
                Item = r,
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string searchString = "", int pageIndex = 1, int pageSize = 20)
        {
            var r = await _categoryService.GetAllPagination(searchString, pageIndex, pageSize);
            return Ok(new ApiResponse<object>
            {
                Item = r,
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryRequest request)
        {
            await _updateCategoryRequestValidator.ValidateAndThrowAsync(request);
            var category = await _categoryService.Update(request);
            return Ok(new ApiResponse<object>
            {
                Item = category,
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var category = await _categoryService.Delete(id);
            return Ok(new ApiResponse<object>
            {
                Item = category,
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMany([FromBody] DeleteManyCategoriesRequest request)
        {
            await _categoryService.DeleteMany(request);
            return Ok(new ApiResponse<object>());
        }
    }
}
