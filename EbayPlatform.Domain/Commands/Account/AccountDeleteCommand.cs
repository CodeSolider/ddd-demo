using MediatR;

namespace EbayPlatform.Domain.Commands.Account
{
    /// <summary>
    /// 删除账单信息
    /// </summary>
    public class AccountDeleteCommand : IRequest<bool>
    {
        public string AccountID { get; }

        public AccountDeleteCommand(string accountID)
        {
            this.AccountID = accountID;
        }
    }
}
