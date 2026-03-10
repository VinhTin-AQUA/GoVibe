using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.Places
{
    public interface IPlaceQueryRepository : IQueryRepository<Place>
    {
        
    }
    
    public class PlaceQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<Place>(contextFactory), IPlaceQueryRepository
    {
        
    }
}