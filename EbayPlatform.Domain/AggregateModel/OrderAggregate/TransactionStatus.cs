using EbayPlatform.Domain.Core.Abstractions;
using System.Collections.Generic;

namespace EbayPlatform.Domain.AggregateModel.OrderAggregate
{
    /// <summary>
    /// 事务状态
    /// </summary>
    public class TransactionStatus : ValueObject
    {
        /// <summary>
        /// 待付款状态
        /// </summary>
        public string PaymentHoldStatus { get; private set; }

        /// <summary>
        /// 查询状态
        /// </summary>
        public string InquiryStatus { get; private set; }

        /// <summary>
        /// 退回状态
        /// </summary>
        public string ReturnStatus { get; private set; }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PaymentHoldStatus;
            yield return InquiryStatus;
            yield return ReturnStatus;
        }


        public TransactionStatus() { }

        public TransactionStatus(string paymentHoldStatus, string inquiryStatus, string returnStatus)
        {
            this.PaymentHoldStatus = paymentHoldStatus;
            this.InquiryStatus = inquiryStatus;
            this.ReturnStatus = returnStatus;
        }
    }
}
