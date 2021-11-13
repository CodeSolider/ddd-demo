using EbayPlatform.Domain.Commands.Account;
using EbayPlatform.Domain.Interfaces;
using MediatR;
using Newtonsoft.Json;
using Serilog.Context;
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
            using (LogContext.PushProperty("AccountDeleteCommand", $"{JsonConvert.SerializeObject(request)}"))
            {
                var accountItem = await _accountRepository.FirstOrDefaultAsync(o => o.AccountID == request.AccountID).ConfigureAwait(false);
                if (accountItem == null)
                {
                    return false;
                }
                this._accountRepository.Remove(accountItem);
                return await _accountRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 创建账单信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(AccountCreatedCommand request, CancellationToken cancellationToken)
        {
            using (LogContext.PushProperty("AccountCreatedCommand", $"{JsonConvert.SerializeObject(request)}"))
            {
                if (request.Account == null)
                {
                    return false;
                }
                _accountRepository.Add(request.Account);
                return await _accountRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
