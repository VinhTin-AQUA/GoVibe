using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.Amenities
{
    public interface IAmenityQueryRepository : IQueryRepository<Amenity>
    {
        Task<(List<Amenity>, int)> GetAllPagination(int pageIndex = 0, int pageSize = 50);
    }
    
    public class AmenityQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<Amenity>(contextFactory), IAmenityQueryRepository
    {
        public async Task<(List<Amenity>, int)> GetAllPagination(int pageIndex = 0, int pageSize = 50)
        {
            var r = await GetPagedAsync(pageIndex, pageSize);
            var total = await CountAsync(x => true);
            return (r, total);
        }
    }
}