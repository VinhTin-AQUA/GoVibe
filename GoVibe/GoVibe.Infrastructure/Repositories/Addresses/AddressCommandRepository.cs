using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Data;
using GoVibe.Infrastructure.Repositories.Common;

namespace GoVibe.Infrastructure.Repositories.Addresses
{
    public interface IAddressCommandRepository : ICommandRepository<Address>
    {
        
    }
    
    public class AddressCommandRepository(AppDbContext context)
        : CommandRepository<Address>(context), IAddressCommandRepository
    {
        
    }
}