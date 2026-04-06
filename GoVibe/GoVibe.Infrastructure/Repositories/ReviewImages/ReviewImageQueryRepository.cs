using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using GoVibe.Infrastructure.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.ReviewImages
{
    public interface IReviewImageQueryRepository : IQueryRepository<ReviewImage>
    {
        Task<List<ReviewImage>> GetListByReviewId(Guid reviewId);
    }
    
    public class ReviewImageQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<ReviewImage>(contextFactory), IReviewImageQueryRepository
    {
        public async Task<List<ReviewImage>> GetListByReviewId(Guid reviewId)
        {
            QueryOptionsBuilder<ReviewImage> builder = new();
            builder.Where(x => x.ReviewId == reviewId);
            builder.AsNoTracking(false);
            
            return await FilterAsync(builder.Build());
        }
    }
}