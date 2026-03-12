using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.Places
{
    public interface IPlaceQueryRepository : IQueryRepository<Place>
    {
        Task<(List<Place>, int)> GetAllPagination(int pageIndex = 0, int pageSize = 50);
    }
    
    public class PlaceQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<Place>(contextFactory), IPlaceQueryRepository
    {
        public async Task<(List<Place>, int)> GetAllPagination(int pageIndex = 0, int pageSize = 50)
        {
            var r = await GetPagedAsync(pageIndex, pageSize);
            var total = await CountAsync(x => true);
            return (r, total);
        }
    }
}