using MediatR;
using System.Collections.Generic;

namespace EbayPlatform.Domain.Commands.Account
{
    public class AccountCreatedCommand : IRequest<bool>
    {
        /// <summary>
        /// 添加账单
        /// </summary>
        public List<Models.Accounts.Account> Accounts { get; }

        public AccountCreatedCommand(List<Models.Accounts.Account> accounts)
        {
            this.Accounts = accounts;
        }
    }
}
