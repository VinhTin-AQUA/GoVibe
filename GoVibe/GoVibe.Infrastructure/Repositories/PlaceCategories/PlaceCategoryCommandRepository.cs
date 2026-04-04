using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;

namespace GoVibe.Infrastructure.Repositories.Places
{
    public interface IPlaceCategoryCommandRepository : ICommandRepository<PlaceCategory>
    {
        
    }
    
    public class PlaceCategoryCommandRepository(AppDbContext context) : CommandRepository<PlaceCategory>(context), IPlaceCategoryCommandRepository
    {
        
    }
}