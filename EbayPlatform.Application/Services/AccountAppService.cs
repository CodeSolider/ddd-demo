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
        public Task<bool> DeleteAccountIdsAsync(IEnumerable<string> accountIDList, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new AccountDeleteCommand(accountIDList), cancellationToken);
        }

        #region Create Account
        /// <summary>
        /// 添加账单信息
        /// </summary>
        /// <param name="accountDtos"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> AddAccountAsync(List<AccountDto> accountDtos, CancellationToken cancellationToken = default)
        {
            List<Domain.Models.Accounts.Account> accountList = new();

            accountDtos.ForEach(accountDtoItem =>
            {
                Domain.Models.Accounts.Account accountItem = new(accountDtoItem.AccountID, accountDtoItem.ShopName, accountDtoItem.CurrencyCode,
                                                                 accountDtoItem.AccountState, new Domain.Models.MoneyValue(accountDtoItem.InvoicePaymentValue,
                                                                 accountDtoItem.InvoicePaymentCurrency), new Domain.Models.MoneyValue(accountDtoItem.InvoiceCreditValue,
                                                                 accountDtoItem.InvoicePaymentCurrency), new Domain.Models.MoneyValue(accountDtoItem.InvoiceNewFeeValue,
                                                                 accountDtoItem.InvoiceNewFeeCurrency), new Domain.Models.Accounts.AdditionalAccount(new Domain.Models.MoneyValue(
                                                                 accountDtoItem.AdditionalAccount.BalanceValue, accountDtoItem.AdditionalAccount.BalanceCurrency),
                                                                 accountDtoItem.AdditionalAccount.CurrencyCode, accountDtoItem.AdditionalAccount.AccountCode));

                accountDtoItem.AccountDetails.ForEach(AccountDetailItem =>
                {
                    accountItem.AddAccountDetail(AccountDetailItem.RefNumber, AccountDetailItem.ItemID, AccountDetailItem.Date, AccountDetailItem.AccountType,
                                                 AccountDetailItem.Title, AccountDetailItem.Description, new Domain.Models.MoneyValue(AccountDetailItem.BalanceValue,
                                                 AccountDetailItem.BalanceCurrency), new Domain.Models.MoneyValue(AccountDetailItem.GrossDetailAmountValue,
                                                 AccountDetailItem.GrossDetailAmountCurrency), new Domain.Models.MoneyValue(AccountDetailItem.ConversionRateValue,
                                                 AccountDetailItem.ConversionRateCurrency), new Domain.Models.MoneyValue(AccountDetailItem.NetDetailAmountValue,
                                                 AccountDetailItem.NetDetailAmountCurrency), AccountDetailItem.VATPercent, AccountDetailItem.OrderLineItemID,
                                                 AccountDetailItem.TransactionID, AccountDetailItem.ReceivedTopRatedDiscount);
                });

                accountList.Add(accountItem);
            });
            return _mediator.Send(new AccountCreatedCommand(accountList), cancellationToken);
        }
        #endregion
    }
}
