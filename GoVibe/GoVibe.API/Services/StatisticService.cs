using GoVibe.API.Models.Categories;
using GoVibe.API.Models.Places;
using GoVibe.API.Models.Statistics;
using GoVibe.Infrastructure.Repositories.Categories;
using GoVibe.Infrastructure.Repositories.PlaceCategories;
using GoVibe.Infrastructure.Repositories.PlaceImages;
using GoVibe.Infrastructure.Repositories.Places;
using GoVibe.Infrastructure.Repositories.ReviewImages;
using GoVibe.Infrastructure.Repositories.Reviews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.API.Services
{
    public class StatisticService
    {
        private readonly IPlaceQueryRepository _placeQueryRepository;
        private readonly ICategoryQueryRepository _categoryQueryRepository;
        private readonly IReviewQueryRepository _reviewQueryRepository;
        private readonly IPlaceImageQueryRepository _placeImageQueryRepository;
        private readonly IReviewImageQueryRepository _reviewImageQueryRepository;
        private readonly IPlaceCategoryQueryRepository _placeCategoryQueryRepository;

        public StatisticService(
            IPlaceQueryRepository placeQueryRepository,
            ICategoryQueryRepository categoryQueryRepository,
            IReviewQueryRepository reviewQueryRepository,
            IPlaceImageQueryRepository placeImageQueryRepository,
            IReviewImageQueryRepository reviewImageQueryRepository,
            IPlaceCategoryQueryRepository placeCategoryQueryRepository
        )
        {
            _placeQueryRepository = placeQueryRepository;
            _categoryQueryRepository = categoryQueryRepository;
            _reviewQueryRepository = reviewQueryRepository;
            _placeImageQueryRepository = placeImageQueryRepository;
            _reviewImageQueryRepository = reviewImageQueryRepository;
            _placeCategoryQueryRepository = placeCategoryQueryRepository;
        }

        public async Task<StatisticOverView> GetOverview(StatisticDateRangeQuery query)
        {
            var (from, to) = query.GetRange();

            var totalPlaces = await _placeQueryRepository.CountAsync(q => q.CreatedAt >= from && q.CreatedAt <= to);
            var totalCategories = await _categoryQueryRepository.CountAsync(x => true);
            var totalReviews = await _reviewQueryRepository.CountAsync(x => x.CreatedAt >= from && x.CreatedAt <= to);
            var totalPlaceImage = await _placeImageQueryRepository.CountAsync(x => x.CreatedAt >= from && x.CreatedAt <= to);
            var totalReviewImage = await _reviewImageQueryRepository.CountAsync(x => x.CreatedAt >= from && x.CreatedAt <= to);
            var allReviews = await _reviewQueryRepository.GetAllAsync(false, q => q.Where(x => x.CreatedAt >= from && x.CreatedAt <= to));
            var avgRating = allReviews.Select(x => (double?)x.Rating).ToList().Average();

            return new StatisticOverView
            {
                TotalPlaces = totalPlaces,
                TotalCategories = totalCategories,
                TotalReviews = totalReviews,
                TotalReviewImage = totalReviewImage,
                TotalPlaceImage = totalPlaceImage,
                AverageRating = avgRating ?? 0
            };
        }

        public async Task<List<RatingDistribution>> GetRatingDistribution(StatisticDateRangeQuery query)
        {
            var (from, to) = query.GetRange();
            var allReviews = await _reviewQueryRepository.GetAllAsync(false, [
                query => query.Where(r => r.CreatedAt >= from && r.CreatedAt <= to),
            ]);

            var data = allReviews
                .GroupBy(x => x.Rating)
                .Select(g => new RatingDistribution
                {
                    Rating = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Rating)
                .ToList();

            return data;
        }

        public async Task<List<PlaceGrowth>> GetPlaceGrowth(StatisticDateRangeQuery query)
        {
            var (from, to) = query.GetRange();
            var places = await _placeQueryRepository.GetAllAsync(false, query => query.Where(r => r.CreatedAt >= from && r.CreatedAt <= to));

            var data = places
                .GroupBy(p => new { p.CreatedAt.Year, p.CreatedAt.Month })
                .Select(g => new PlaceGrowth
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            return data;
        }

        public async Task<List<ReviewGrowth>> GetReviewGrowth(StatisticDateRangeQuery query)
        {
            var (from, to) = query.GetRange();
            var reviews = await _reviewQueryRepository.GetAllAsync(false, [
               query => query.Where(r => r.CreatedAt >= from && r.CreatedAt <= to),
            ]);

            var data = reviews
                .GroupBy(r => new { r.CreatedAt.Year, r.CreatedAt.Month })
                .Select(g => new ReviewGrowth
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            return data;
        }

        public async Task<List<TopCategory>> GetTopCategories()
        {
            var categories = await _placeCategoryQueryRepository.GetAllAsync(false, query => query.Include(x => x.Category));
            var data = categories
                .GroupBy(pc => pc.Category!.Name)
                .Select(g => new TopCategory
                {
                    CategoryName = g.Key,
                    AvgRating = g.Average(x =>
                        x.Place!.TotalReviews == 0
                        ? 0
                        : x.Place.TotalRating / x.Place.TotalReviews)
                })
                .OrderByDescending(x => x.AvgRating)
                .ToList();

            return data;
        }

        public async Task<StatisticSummary> GetSummary()
        {
            var places = await _placeQueryRepository.GetAllAsync(false);
            var topRatedPlaces = places
                .Select(p => new PlaceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    TotalReviews = p.TotalReviews,
                    TotalViews = p.TotalViews,
                    AverageRating = p.TotalReviews == 0
                        ? 0
                        : p.TotalRating / p.TotalReviews
                })
                .OrderByDescending(p => p.AverageRating)
                .Take(5)
                .ToList();

            var mostViewedPlaces = places
                .Select(p => new PlaceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    TotalViews = p.TotalViews,
                    TotalReviews = p.TotalReviews,
                    AverageRating = p.TotalReviews == 0
                        ? 0
                        : p.TotalRating / p.TotalReviews
                })
                .OrderByDescending(p => p.TotalViews)
                .Take(5)
                .ToList();

            var placeCategories = await _placeCategoryQueryRepository.GetAllAsync(false, query => query.Include(x => x.Category));
            var placesPerCategory = placeCategories
                .GroupBy(pc => new { pc.CategoryId, pc.Category?.Name })
                .Select(g => new CategoryStats
                {
                    Id = g.Key.CategoryId.ToString(),
                    CategoryName = g.Key.Name ?? "No Name",
                    PlaceCount = g.Select(x => x.PlaceId).Distinct().Count()
                })
                .OrderByDescending(x => x.PlaceCount)
                .ToList();

            return new StatisticSummary
            {
                TopRatedPlaces = topRatedPlaces,
                MostViewedPlaces = mostViewedPlaces,
                PlacesPerCategory = placesPerCategory
            };
        }
    }
}
