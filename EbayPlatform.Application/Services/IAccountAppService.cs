using EbayPlatform.Application.Dtos.Accounts;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Services
{
    public interface IAccountAppService
    {
        /// <summary>
        /// 根据账户ID删除账户数据
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteAccountAsync(string accountID, CancellationToken cancellationToken = default);

        /// <summary>
        /// 添加账单信息
        /// </summary>
        /// <param name="accountDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AddAccountAsync(AccountDto accountDto, CancellationToken cancellationToken = default);
    }
}
