using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.Reviews
{
    public interface IReviewQueryRepository : IQueryRepository<Review>
    {
        
    }
    
    public class ReviewQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<Review>(contextFactory), IReviewQueryRepository
    {
        
    }
}