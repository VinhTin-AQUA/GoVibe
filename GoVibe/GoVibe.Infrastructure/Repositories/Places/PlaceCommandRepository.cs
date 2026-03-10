using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;

namespace GoVibe.Infrastructure.Repositories.Places
{
    public interface IPlaceCommandRepository : ICommandRepository<Place>
    {
        
    }
    
    public class PlaceCommandRepository(AppDbContext context) : CommandRepository<Place>(context), IPlaceCommandRepository
    {
        
    }
}