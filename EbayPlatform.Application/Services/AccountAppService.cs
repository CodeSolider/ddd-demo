using EbayPlatform.Application.Dtos.Accounts;
using EbayPlatform.Domain.Commands.Account;
using EbayPlatform.Domain.Core.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Services
{
    public class AccountAppService : IAccountAppService, IDependency
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
            Domain.Models.Accounts.Account accountItem = new(accountDto.AccountID, accountDto.ShopName, accountDto.CurrencyCode,
                                                                   accountDto.AccountState, new Domain.Models.MoneyValue(accountDto.InvoicePaymentValue,
                                                                   accountDto.InvoicePaymentCurrency), new Domain.Models.MoneyValue(accountDto.InvoiceCreditValue,
                                                                   accountDto.InvoicePaymentCurrency), new Domain.Models.MoneyValue(accountDto.InvoiceNewFeeValue,
                                                                   accountDto.InvoiceNewFeeCurrency), new Domain.Models.Accounts.AdditionalAccount(new Domain.Models.MoneyValue(
                                                                   accountDto.AdditionalAccount.BalanceValue, accountDto.AdditionalAccount.BalanceCurrency),
                                                                   accountDto.AdditionalAccount.CurrencyCode, accountDto.AdditionalAccount.AccountCode));

            accountDto.AccountDetails.ForEach(AccountDetailItem =>
            {
                accountItem.AddAccountDetail(AccountDetailItem.RefNumber, AccountDetailItem.ItemID, AccountDetailItem.Date, AccountDetailItem.AccountType,
                                             AccountDetailItem.Title, AccountDetailItem.Description, new Domain.Models.MoneyValue(AccountDetailItem.BalanceValue,
                                             AccountDetailItem.BalanceCurrency), new Domain.Models.MoneyValue(AccountDetailItem.GrossDetailAmountValue,
                                             AccountDetailItem.GrossDetailAmountCurrency), new Domain.Models.MoneyValue(AccountDetailItem.ConversionRateValue,
                                             AccountDetailItem.ConversionRateCurrency), new Domain.Models.MoneyValue(AccountDetailItem.NetDetailAmountValue,
                                             AccountDetailItem.NetDetailAmountCurrency), AccountDetailItem.VATPercent, AccountDetailItem.OrderLineItemID,
                                             AccountDetailItem.TransactionID, AccountDetailItem.ReceivedTopRatedDiscount);
            });

            return _mediator.Send(new AccountCreatedCommand(accountItem), cancellationToken);
        }
        #endregion
    }
}
