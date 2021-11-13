using MediatR;

namespace EbayPlatform.Domain.Commands.Account
{
    public class AccountCreatedCommand : IRequest<bool>
    {
        /// <summary>
        /// 添加账单
        /// </summary>
        public Models.Accounts.Account Account { get; }

        public AccountCreatedCommand(Models.Accounts.Account account)
        {
            this.Account = account;
        }
    }
}
