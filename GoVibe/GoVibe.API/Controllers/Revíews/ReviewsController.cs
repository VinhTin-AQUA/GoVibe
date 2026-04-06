using AutoMapper;
using GoVibe.API.Controllers.Common;
using GoVibe.API.Models;
using GoVibe.API.Models.Reviews;
using GoVibe.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoVibe.API.Controllers.Revíews
{
    public class ReviewsController : ControllerBaseApi
    {
        private readonly ReviewService _reviewService;

        public ReviewsController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddReviewRequest  request)
        {
            var r = await _reviewService.Add(request);
            return Ok(new ApiResponse<object>
            {
                Item = r
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageIndex = 1, int pageSize = 20)
        {
            var r = await _reviewService.GetAllPagination(pageIndex, pageSize);
            return Ok(new ApiResponse<object>
            {
                Item = r
            });
        }
        
        // delete review
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(string id)
        {
            var r = await _reviewService.DeleteReview(id);
            return Ok(new ApiResponse<object>
            {
                Item = r
            });
        }
        
        // update review
        [HttpPut]
        public async Task<IActionResult> UpdateReivew([FromForm] UpdateReviewModel model)
        {
            var r = await _reviewService.EditReview(model);
            return Ok(new ApiResponse<object>
            {
                Item = r
            });
        }
    }
}