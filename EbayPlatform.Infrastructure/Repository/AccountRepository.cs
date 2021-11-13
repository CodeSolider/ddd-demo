using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Domain.Models.Accounts;
using EbayPlatform.Infrastructure.Context; 

namespace EbayPlatform.Infrastructure.Repository
{
    public class AccountRepository : Repository<Account, long, EbayPlatformDbContext>,
        IAccountRepository, IDependency
    {
        public AccountRepository(EbayPlatformDbContext dbContext)
        : base(dbContext) { }

    }
}
