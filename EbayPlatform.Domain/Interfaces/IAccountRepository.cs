using EbayPlatform.Domain.AggregateModel.AccountAggregate;
using EbayPlatform.Domain.Core.Abstractions;

namespace EbayPlatform.Domain.Interfaces
{
    public interface IAccountRepository : IRepository<Account, long>
    {
    }
}
