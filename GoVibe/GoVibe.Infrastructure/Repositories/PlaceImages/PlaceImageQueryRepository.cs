using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.PlaceImages
{
    public interface IPlaceImageQueryRepository : IQueryRepository<PlaceImage>
    {
        
    }
    
    public class PlaceImageQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<PlaceImage>(contextFactory), IPlaceImageQueryRepository
    {
        
    }
}