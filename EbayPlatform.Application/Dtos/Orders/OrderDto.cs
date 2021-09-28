using System;
using System.Collections.Generic;

namespace EbayPlatform.Application.Dtos.Orders
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OrderDto
    {
        public OrderDto()
        {
            OrderTransactions = new List<OrderTransactionDto>();
        }

        /// <summary>
        /// eBay 下载的订单ID、唯一
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus { get; set; }

        #region AdjustmentAmount
        /// <summary>
        /// 或者值
        /// </summary>
        public decimal AdjustmentAmountValue { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string AdjustmentAmountCurrency { get; set; }
        #endregion

        #region AmountPaid
        /// <summary>
        /// 或者值
        /// </summary>
        public decimal AmountPaidValue { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string AmountPaidCurrency { get; set; }
        #endregion

        #region AmountSaved
        /// <summary>
        /// 或者值
        /// </summary>
        public decimal AmountSavedValue { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string AmountSavedCurrency { get; set; }
        #endregion 

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PaymentMethods { get; set; }

        /// <summary>
        /// 卖家邮箱地址
        /// </summary>
        public string SellerEmail { get; set; }

        /// <summary>
        /// 卖家账号
        /// </summary>
        public string SellerUserID { get; set; }

        /// <summary>
        /// 订单创建日期
        /// </summary>
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// 订单同步日期
        /// </summary>
        public DateTime SyncDate { get; set; }

        /// <summary>
        /// 订单交易
        /// </summary>
        public List<OrderTransactionDto> OrderTransactions { get; set; }

        #region Subtotal
        /// <summary>
        /// 或者值
        /// </summary>
        public decimal SubtotalValue { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string SubtotalCurrency { get; set; }
        #endregion

        #region Total
        /// <summary>
        /// 或者值
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string TotalCurrency { get; set; }
        #endregion

        #region CheckOutStatus
        public string EBayPaymentStatus { get; set; }

        /// <summary>
        /// 结账支付方式
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// IntegratedMerchantCreditCardEnabled
        /// </summary>
        public bool? IntegratedMerchantCreditCardEnabled { get; set; }

        /// <summary>
        /// 支付工具
        /// </summary>
        public string PaymentInstrument { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModifiedTime { get; set; }
        #endregion 

        /// <summary>
        /// 装运详情
        /// </summary>
        public ShippingDetailDto ShippingDetail { get; set; }

        /// <summary>
        /// 发货地址
        /// </summary>
        public AddressDto ShippingAddress { get; set; }

        /// <summary>
        /// 发货选中服务
        /// </summary>
        public ShippingServiceOptionDto ShippingServiceSelected { get; set; }
    }
}
