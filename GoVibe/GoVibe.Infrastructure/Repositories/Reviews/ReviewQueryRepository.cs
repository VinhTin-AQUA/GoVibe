using System.Linq.Expressions;
using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using GoVibe.Infrastructure.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.Reviews
{
    public interface IReviewQueryRepository : IQueryRepository<Review>
    {
        Task<(List<Review>, int)> GetAllPagination(int pageIndex = 0, int pageSize = 50);
    }
    
    public class ReviewQueryRepository(IDbContextFactory<AppDbContext> contextFactory) : QueryRepository<Review>(contextFactory), IReviewQueryRepository
    {
        public async Task<(List<Review>, int)> GetAllPagination(int pageIndex = 0, int pageSize = 50)
        {
            QueryOptionsBuilder<Review> builder = new();
            Expression<Func<Review, bool>> predicate = x => true;

            builder.Include(x => x.Images);

            builder.AsNoTracking()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            var r = await FilterAsync(builder.Build());
            var total = await CountAsync(predicate);
            return (r, total);
        }
    }
}