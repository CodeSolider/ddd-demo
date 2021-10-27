using MediatR;
using System.Collections.Generic;

namespace EbayPlatform.Domain.Commands.Account
{
    /// <summary>
    /// 删除账单信息
    /// </summary>
    public class AccountDeleteCommand : IRequest<bool>
    {
        public IEnumerable<string> AccountIDList { get; }

        public AccountDeleteCommand(IEnumerable<string> accountIDList)
        {
            this.AccountIDList = accountIDList;
        }
    }
}
