using MediatR;

namespace EbayPlatform.Domain.Commands.Account
{
    public class AccountCreatedCommand : IRequest<bool>
    {
        /// <summary>
        /// 添加账单
        /// </summary>
        public AggregateModel.AccountAggregate.Account Account { get; }

        public AccountCreatedCommand(AggregateModel.AccountAggregate.Account account)
        {
            this.Account = account;
        }
    }
}
