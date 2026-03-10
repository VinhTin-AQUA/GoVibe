using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.ReviewImages
{
    public interface IReviewImageQueryRepository : IQueryRepository<ReviewImage>
    {
        
    }
    
    public class ReviewImageQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<ReviewImage>(contextFactory), IReviewImageQueryRepository
    {
        
    }
}