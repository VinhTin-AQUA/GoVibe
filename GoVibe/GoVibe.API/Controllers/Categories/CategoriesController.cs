using GoVibe.API.Controllers.Common;
using GoVibe.API.Models;
using GoVibe.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoVibe.API.Controllers.Categories
{
    public class CategoriesController : ControllerBaseApi
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("options")]
        public async Task<IActionResult> GetOptions()
        {
            var options = await _categoryService.GetOptions();
            return Ok(new ApiResponse<object>
            {
                Item = options
            });
        }
    }
}
