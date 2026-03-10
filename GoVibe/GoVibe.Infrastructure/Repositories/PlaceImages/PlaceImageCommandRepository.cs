using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;

namespace GoVibe.Infrastructure.Repositories.PlaceImages
{
    public interface IPlaceImageCommandRepository : ICommandRepository<PlaceImage>
    {
        
    }
    
    public class PlaceImageCommandRepository(AppDbContext context) : CommandRepository<PlaceImage>(context), IPlaceImageCommandRepository
    {
        
    }
}