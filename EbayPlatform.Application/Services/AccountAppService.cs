using EbayPlatform.Application.Dtos.Accounts;
using EbayPlatform.Domain.Commands.Account;
using EbayPlatform.Domain.Core.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Services
{
    public class AccountAppService : IAccountAppService, IScopedDependency
    {
        private readonly IMediator _mediator;
        public AccountAppService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 根据账户ID删除账户数据
        /// </summary>
        /// <param name="accountIDList"></param>
        /// <returns></returns>
        public Task<bool> DeleteAccountAsync(string accountID, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new AccountDeleteCommand(accountID), cancellationToken);
        }

        #region Create Account
        /// <summary>
        /// 添加账单信息
        /// </summary>
        /// <param name="accountDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> AddAccountAsync(AccountDto accountDto, CancellationToken cancellationToken = default)
        {
            Domain.AggregateModel.AccountAggregate.Account accountItem = new(accountDto.AccountID, accountDto.ShopName, accountDto.CurrencyCode,
                                                                   accountDto.AccountState, new Domain.AggregateModel.MoneyValue(accountDto.InvoicePaymentValue,
                                                                   accountDto.InvoicePaymentCurrency), new Domain.AggregateModel.MoneyValue(accountDto.InvoiceCreditValue,
                                                                   accountDto.InvoicePaymentCurrency), new Domain.AggregateModel.MoneyValue(accountDto.InvoiceNewFeeValue,
                                                                   accountDto.InvoiceNewFeeCurrency), new Domain.AggregateModel.AccountAggregate.AdditionalAccount(new Domain.AggregateModel.MoneyValue(
                                                                   accountDto.AdditionalAccount.BalanceValue, accountDto.AdditionalAccount.BalanceCurrency),
                                                                   accountDto.AdditionalAccount.CurrencyCode, accountDto.AdditionalAccount.AccountCode));

            accountDto.AccountDetails.ForEach(AccountDetailItem =>
            {
                accountItem.AddAccountDetail(AccountDetailItem.RefNumber, AccountDetailItem.ItemID, AccountDetailItem.Date, AccountDetailItem.AccountType,
                                             AccountDetailItem.Title, AccountDetailItem.Description, new Domain.AggregateModel.MoneyValue(AccountDetailItem.BalanceValue,
                                             AccountDetailItem.BalanceCurrency), new Domain.AggregateModel.MoneyValue(AccountDetailItem.GrossDetailAmountValue,
                                             AccountDetailItem.GrossDetailAmountCurrency), new Domain.AggregateModel.MoneyValue(AccountDetailItem.ConversionRateValue,
                                             AccountDetailItem.ConversionRateCurrency), new Domain.AggregateModel.MoneyValue(AccountDetailItem.NetDetailAmountValue,
                                             AccountDetailItem.NetDetailAmountCurrency), AccountDetailItem.VATPercent, AccountDetailItem.OrderLineItemID,
                                             AccountDetailItem.TransactionID, AccountDetailItem.ReceivedTopRatedDiscount);
            });

            return _mediator.Send(new AccountCreatedCommand(accountItem), cancellationToken);
        }
        #endregion
    }
}
