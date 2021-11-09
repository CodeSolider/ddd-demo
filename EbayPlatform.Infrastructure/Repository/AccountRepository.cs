using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Domain.Models.Accounts;
using EbayPlatform.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Repository
{
    public class AccountRepository : Repository<Account, long, EbayPlatformDbContext>,
        IAccountRepository, IDependency
    {
        public AccountRepository(EbayPlatformDbContext dbContext)
        : base(dbContext) { }


        /// <summary>
        /// 根据账单ID获取所有的账单数据
        /// </summary>
        /// <param name="accountIDList"></param>
        /// <returns></returns>
        public async Task<List<Account>> GetAccountListByOrderIdsAsync(IEnumerable<string> accountIDList)
        {
            return await this.DbContext.Accounts.IgnoreAutoIncludes()
                             .Where(o => accountIDList.Contains(o.AccountID))
                             .ToListAsync()
                             .ConfigureAwait(false);
        }
    }
}
