using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;

namespace GoVibe.Infrastructure.Repositories.Categories
{
    public interface ICategoryCommandRepository : ICommandRepository<Category>
    {
        
    }
    
    public class CategoryCommandRepository(AppDbContext context) : CommandRepository<Category>(context), ICategoryCommandRepository
    {
        
    }
}