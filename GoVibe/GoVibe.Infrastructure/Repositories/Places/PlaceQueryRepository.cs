using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using GoVibe.Infrastructure.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GoVibe.Infrastructure.Repositories.Places
{
    public interface IPlaceQueryRepository : IQueryRepository<Place>
    {
        Task<(List<Place>, int)> GetAllPagination(int pageIndex = 0, int pageSize = 50);
        Task<Place?> GetPlaceByIdIncludePlaceAmenities(Guid id);
    }
    
    public class PlaceQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<Place>(contextFactory), IPlaceQueryRepository
    {
        public async Task<(List<Place>, int)> GetAllPagination(int pageIndex = 0, int pageSize = 50)
        {
            var r = await GetPagedAsync(pageIndex, pageSize);
            var total = await CountAsync(x => true);
            return (r, total);
        }

        public async Task<Place?> GetPlaceByIdIncludePlaceAmenities(Guid id)
        {
            QueryOptionsBuilder<Place> query = new();
            query.Where(x => x.Id == id);
            query.Include(x => x.PlaceAmenities);

            return await Find(query.Build());
        }
    }
}