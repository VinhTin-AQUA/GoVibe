using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;

namespace GoVibe.Infrastructure.Repositories.ReviewImages
{
    public interface IReviewImageCommandRepository : ICommandRepository<ReviewImage>
    {
        
    }
    
    public class ReviewImageCommandRepository(AppDbContext context) : CommandRepository<ReviewImage>(context), IReviewImageCommandRepository
    {
        
    }
}