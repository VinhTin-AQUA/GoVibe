using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.Infrastructure.Repositories.Addresses
{
    public interface IAddressQueryRepository : IQueryRepository<Address>
    {
        
    }
    
    public class AddressQueryRepository(IDbContextFactory<AppDbContext> contextFactory)
        : QueryRepository<Address>(contextFactory), IAddressQueryRepository
    {
        
    }
}