using GoVibe.API.Controllers.Common;
using GoVibe.API.Models;
using GoVibe.API.Models.Categories;
using GoVibe.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoVibe.API.Controllers.Categories
{
    public class CategoriesController : ControllerBaseApi
    {
        private readonly CategoryService categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryRequest request)
        {
            var r = await categoryService.Add(request);
            return Ok(new ApiResponse<object>
            {
                Item = r
            });
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var r = await categoryService.GetById(id);
            return Ok(new ApiResponse<object>
            {
                Item = r,
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string searchString = "", int pageIndex = 1, int pageSize = 20)
        {
            var r = await categoryService.GetAllPagination(searchString, pageIndex, pageSize);
            return Ok(new ApiResponse<object>
            {
                Item = r,
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryRequest request)
        {
            var category = await categoryService.Update(request);
            return Ok(new ApiResponse<object>
            {
                Item = category,
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var category = await categoryService.Delete(id);
            return Ok(new ApiResponse<object>
            {
                Item = category,
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMany([FromBody] DeleteManyCategoriesRequest request)
        {
            await categoryService.DeleteMany(request);
            return Ok(new ApiResponse<object>());
        }
    }
}
