using EbayPlatform.Domain.Core.Abstractions;
using System;
using System.Collections.Generic;

namespace EbayPlatform.Domain.AggregateModel.OrderAggregate
{
    /// <summary>
    /// 订单结账状态
    /// </summary>
    public class CheckoutStatus : ValueObject
    {
        public string EBayPaymentStatus { get; private set; }

        /// <summary>
        /// 结账支付方式
        /// </summary>
        public string PaymentMethod { get; private set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; private set; }

        /// <summary>
        /// IntegratedMerchantCreditCardEnabled
        /// </summary>
        public bool? IntegratedMerchantCreditCardEnabled { get; private set; }

        /// <summary>
        /// 支付工具
        /// </summary>
        public string PaymentInstrument { get; private set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModifiedTime { get; private set; }



        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EBayPaymentStatus;
            yield return PaymentMethod;
            yield return Status;
            yield return IntegratedMerchantCreditCardEnabled;
            yield return PaymentInstrument;
            yield return LastModifiedTime;
        }


        public CheckoutStatus() { }

        public CheckoutStatus(string eBayPaymentStatus, string paymentMethod, string status,
            bool? integratedMerchantCreditCardEnabled, string paymentInstrument, DateTime? lastModifiedTime)
        {
            this.EBayPaymentStatus = eBayPaymentStatus;
            this.PaymentMethod = paymentMethod;
            this.Status = status;
            this.IntegratedMerchantCreditCardEnabled = integratedMerchantCreditCardEnabled;
            this.PaymentInstrument = paymentInstrument;
            this.LastModifiedTime = lastModifiedTime;
        }
    }
}
