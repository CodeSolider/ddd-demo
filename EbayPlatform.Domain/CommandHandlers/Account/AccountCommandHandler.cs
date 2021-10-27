using EbayPlatform.Domain.Commands.Account;
using EbayPlatform.Domain.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.CommandHandlers.Account
{
    /// <summary>
    /// 账单操作
    /// </summary>
    public class AccountCommandHandler : IRequestHandler<AccountDeleteCommand, bool>,
        IRequestHandler<AccountCreatedCommand, bool>
    {
        private readonly IAccountRepository _accountRepository;
        public AccountCommandHandler(IAccountRepository accountRepository)
        {
            this._accountRepository = accountRepository;
        }

        /// <summary>
        /// 根据账单ID删除账单信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(AccountDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.AccountIDList.Any())
            {
                return await Task.FromResult(false);
            }

            var accountList = await _accountRepository
                                    .GetAccountListByOrderIdsAsync(request.AccountIDList)
                                    .ConfigureAwait(false);            
            if (!accountList.Any())
            {
                return false;
            }
            this._accountRepository.RemoveRange(accountList);
            return await _accountRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 创建账单信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> Handle(AccountCreatedCommand request, CancellationToken cancellationToken)
        {
            if (!request.Accounts.Any())
            {
                return Task.FromResult(false);
            }
            _accountRepository.AddRange(request.Accounts);
            return _accountRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        /// <summary>
        /// 手动释放
        /// </summary>
        public void Dispose()
        {
            _accountRepository.Dispose();
        }
    }

}
