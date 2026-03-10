using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;

namespace GoVibe.Infrastructure.Repositories.Reviews
{
    public interface IReviewCommandRepository : ICommandRepository<Review>
    {
        
    }
    
    public class ReviewCommandRepository(AppDbContext context) : CommandRepository<Review>(context), IReviewCommandRepository
    {
        
    }
}