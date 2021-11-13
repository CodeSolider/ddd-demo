using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Models.Accounts; 

namespace EbayPlatform.Domain.Interfaces
{
    public interface IAccountRepository : IRepository<Account, long>
    {
    }
}
