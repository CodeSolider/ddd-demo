using EbayPlatform.Domain.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EbayPlatform.Domain.AggregateModel.AccountAggregate
{
    /// <summary>
    /// 账单
    /// </summary>
    public class Account : Entity<long>, IAggregateRoot
    {
        /// <summary>
        /// 账单ID
        /// </summary>
        public string AccountID { get; private set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; private set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string CurrencyCode { get; private set; }

        /// <summary>
        /// 账单状态
        /// </summary>
        public string AccountState { get; private set; }

        /// <summary>
        /// 发票付款
        /// </summary>
        public MoneyValue InvoicePayment { get; private set; }

        /// <summary>
        /// 发票信用证
        /// </summary>
        public MoneyValue InvoiceCredit { get; private set; }

        /// <summary>
        /// 开具新费用发票
        /// </summary>
        public MoneyValue InvoiceNewFee { get; private set; }

        /// <summary>
        /// 额外账户
        /// </summary>
        public AdditionalAccount AdditionalAccount { get; private set; }

        /// <summary>
        /// 同步日期
        /// </summary>
        public DateTime SyncDate { get; private set; }

        /// <summary>
        /// 账单明细
        /// </summary>
        public virtual ICollection<AccountDetail> AccountDetails { get; private set; }

        protected Account()
        {
            AccountDetails = new List<AccountDetail>();
        }

        public Account(string accountID, string shopName, string currencyCode,
            string accountState, MoneyValue invoicePayment, MoneyValue invoiceCredit,
            MoneyValue invoiceNewFee, AdditionalAccount additionalAccount)
        {
            this.AccountID = accountID;
            this.ShopName = shopName;
            this.CurrencyCode = currencyCode;
            this.AccountState = accountState;
            this.InvoicePayment = invoicePayment;
            this.InvoiceCredit = invoiceCredit;
            this.InvoiceNewFee = invoiceNewFee;
            this.AdditionalAccount = additionalAccount;

            ////添加事件
            //this.AddDomainEvent(new CreateAccountDomainEvent(this));
        }

        /// <summary>
        /// 添加账单明细数据
        /// </summary>
        /// <param name="refNumber"></param>
        /// <param name="itemID"></param>
        /// <param name="date"></param>
        /// <param name="accountType"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="balance"></param>
        /// <param name="grossDetailAmount"></param>
        /// <param name="conversionRate"></param>
        /// <param name="netDetailAmount"></param>
        /// <param name="vatPercent"></param>
        /// <param name="orderLineItemID"></param>
        /// <param name="transactionID"></param>
        /// <param name="receivedTopRatedDiscount"></param>
        public void AddAccountDetail(string refNumber, string itemID, DateTime date,
            string accountType, string title, string description,
            MoneyValue balance, MoneyValue grossDetailAmount, MoneyValue conversionRate,
            MoneyValue netDetailAmount, decimal vatPercent, string orderLineItemID,
            string transactionID, bool receivedTopRatedDiscount)
        {
            var existsAccountDetailItem = this.AccountDetails.FirstOrDefault(o => o.RefNumber == refNumber && o.ItemID == itemID && o.Date == date && o.AccountType == accountType);
            if (existsAccountDetailItem != null)
            {
                existsAccountDetailItem.ChangeAccountDetail(title, description, balance, grossDetailAmount, conversionRate, netDetailAmount, vatPercent, receivedTopRatedDiscount);
            }
            else
            {
                this.AccountDetails.Add(new AccountDetail(refNumber, itemID, date, accountType, title, description, balance, grossDetailAmount,
                                        conversionRate, netDetailAmount, vatPercent, orderLineItemID, transactionID, receivedTopRatedDiscount));
            }

        }


    }
}
