using GoVibe.API.Controllers.Common;
using GoVibe.API.Models;
using GoVibe.API.Models.Places;
using GoVibe.API.Models.Statistics;
using GoVibe.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GoVibe.API.Controllers.Statistics
{
    public class StatisticsController : ControllerBaseApi
    {
        private readonly StatisticService _statisticService;

        public StatisticsController(StatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [HttpPost("overview")]
        public async Task<IActionResult> GetOverview([FromBody] StatisticDateRangeQuery query)
        {
            var overview = await _statisticService.GetOverview(query);
            return Ok(new ApiResponse<object>
            {
                Item = overview
            });
        }

        [HttpPost("rating-distribution")]
        public async Task<IActionResult> GetRatingDistribution([FromBody] StatisticDateRangeQuery query)
        {
            var distribution = await _statisticService.GetRatingDistribution(query);
            return Ok(new ApiResponse<object>
            {
                Item = distribution
            });
        }


        [HttpPost("place-growth")]
        public async Task<IActionResult> GetPlaceGrowth([FromBody] StatisticDateRangeQuery query)
        {
            var growth = await _statisticService.GetPlaceGrowth(query);
            return Ok(new ApiResponse<object>
            {
                Item = growth
            });
        }

        [HttpPost("review-growth")]
        public async Task<IActionResult> GetReviewGrowth([FromBody] StatisticDateRangeQuery query)
        {
            var growth = await _statisticService.GetReviewGrowth(query);
            return Ok(new ApiResponse<object>
            {
                Item = growth
            });
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _statisticService.GetSummary();
            return Ok(new ApiResponse<object>
            {
                Item = summary
            });
        }
    }
}
