using EbayPlatform.Application.Dtos.Accounts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Services
{
    public interface IAccountAppService
    {
        /// <summary>
        /// 根据账户ID删除账户数据
        /// </summary>
        /// <param name="accountIDList"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteAccountIdsAsync(IEnumerable<string> accountIDList, CancellationToken cancellationToken = default);

        /// <summary>
        /// 添加账单信息
        /// </summary>
        /// <param name="accountDtos"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AddAccountAsync(List<AccountDto> accountDtos, CancellationToken cancellationToken = default);
    }
}
