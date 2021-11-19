using EbayPlatform.Domain.AggregateModel.AccountAggregate;
using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Infrastructure.Context;

namespace EbayPlatform.Infrastructure.Repository
{
    public class AccountRepository : Repository<Account, long, EbayPlatformDbContext>,
        IAccountRepository, IScopedDependency
    {
        public AccountRepository(EbayPlatformDbContext dbContext)
        : base(dbContext) { }

    }
}
