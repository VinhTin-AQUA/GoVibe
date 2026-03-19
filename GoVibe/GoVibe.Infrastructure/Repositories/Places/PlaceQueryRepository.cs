using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using GoVibe.Infrastructure.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GoVibe.Infrastructure.Repositories.Places
{
    public interface IPlaceQueryRepository : IQueryRepository<Place>
    {
        Task<(List<Place>, int)> GetAllPagination(string searchString = "", int pageIndex = 0, int pageSize = 50);
    }
    
    public class PlaceQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<Place>(contextFactory), IPlaceQueryRepository
    {
        public async Task<(List<Place>, int)> GetAllPagination(string searchString = "", int pageIndex = 0, int pageSize = 50)
        {
            QueryOptionsBuilder<Place> builder = new();
            Expression<Func<Place, bool>> predicate = x => true;

            if (!string.IsNullOrEmpty(searchString))
            {
                builder.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
                predicate = p => p.Name.ToLower().Contains(searchString.ToLower());
            }

            builder.AsNoTracking()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            var r = await FilterAsync(builder.Build());
            var total = await CountAsync(predicate);
            return (r, total);
        }
    }
}