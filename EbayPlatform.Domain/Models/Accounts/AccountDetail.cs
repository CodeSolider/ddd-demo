using EbayPlatform.Domain.Core.Abstractions;
using System;

namespace EbayPlatform.Domain.Models.Accounts
{
    /// <summary>
    /// 账单明细
    /// </summary>
    public class AccountDetail : Entity<long>
    {
        /// <summary>
        /// 参数
        /// </summary>
        public string RefNumber { get; private set; }

        /// <summary>
        /// ItemID
        /// </summary>
        public string ItemID { get; private set; }

        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// 账单类型
        /// </summary>
        public string AccountType { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 余额
        /// </summary>
        public MoneyValue Balance { get; private set; }

        /// <summary>
        /// 总额
        /// </summary>
        public MoneyValue GrossDetailAmount { get; private set; }

        /// <summary>
        /// 转化率
        /// </summary>
        public MoneyValue ConversionRate { get; private set; }

        /// <summary>
        /// 净额
        /// </summary>
        public MoneyValue NetDetailAmount { get; private set; }

        /// <summary>
        /// 百分比增值税
        /// </summary>
        public decimal VATPercent { get; private set; }

        /// <summary>
        /// OrderLineItemID
        /// </summary>
        public string OrderLineItemID { get; private set; }

        /// <summary>
        /// TransactionID
        /// </summary>
        public string TransactionID { get; private set; }

        /// <summary>
        /// 是否收取额外费用
        /// </summary>
        public bool ReceivedTopRatedDiscount { get; private set; }

        public AccountDetail() { }

        public AccountDetail(string refNumber, string itemID, DateTime date,
            string accountType, string title, string description,
            MoneyValue balance, MoneyValue grossDetailAmount, MoneyValue conversionRate,
            MoneyValue netDetailAmount, decimal vatPercent, string orderLineItemID,
            string transactionID, bool receivedTopRatedDiscount)
        {
            this.RefNumber = refNumber;
            this.ItemID = itemID;
            this.Date = Convert.ToDateTime(date.ToString("yyyy-MM-dd"));
            this.AccountType = accountType;
            this.Title = title;
            this.Description = description;
            this.Balance = balance;
            this.GrossDetailAmount = grossDetailAmount;
            this.ConversionRate = conversionRate;
            this.NetDetailAmount = netDetailAmount;
            this.VATPercent = vatPercent;
            this.OrderLineItemID = orderLineItemID;
            this.TransactionID = transactionID;
            this.ReceivedTopRatedDiscount = receivedTopRatedDiscount;
        }


        /// <summary>
        /// 更新账户明细信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="balance"></param>
        /// <param name="grossDetailAmount"></param>
        /// <param name="conversionRate"></param>
        /// <param name="netDetailAmount"></param>
        /// <param name="vatPercent"></param>
        /// <param name="receivedTopRatedDiscount"></param>
        public void ChangeAccountDetail(string title, string description, MoneyValue balance, MoneyValue grossDetailAmount,
           MoneyValue conversionRate, MoneyValue netDetailAmount, decimal vatPercent, bool receivedTopRatedDiscount)
        {
            this.Title = title;
            this.Description = description;
            this.Balance = balance;
            this.GrossDetailAmount = grossDetailAmount;
            this.ConversionRate = conversionRate;
            this.NetDetailAmount = netDetailAmount;
            this.VATPercent = vatPercent;
            this.ReceivedTopRatedDiscount = receivedTopRatedDiscount;
        }

    }
}
