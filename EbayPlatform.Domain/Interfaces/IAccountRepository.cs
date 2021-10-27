using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Models.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.Interfaces
{
    public interface IAccountRepository : IRepository<Account, long>
    {
        /// <summary>
        /// 根据账单ID获取所有的账单数据
        /// </summary>
        /// <param name="accountIDList"></param>
        /// <returns></returns>
        Task<List<Account>> GetAccountListByOrderIdsAsync(IEnumerable<string> accountIDList);
    }
}
