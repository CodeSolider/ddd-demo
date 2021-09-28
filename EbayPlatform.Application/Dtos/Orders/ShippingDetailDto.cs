using System.Collections.Generic;

namespace EbayPlatform.Application.Dtos.Orders
{
    public class ShippingDetailDto
    {
        public ShippingDetailDto()
        {
            ShippingServiceOptions = new List<ShippingServiceOptionDto>();
        }

        #region SalesTax
        /// <summary>
        /// 营业税率
        /// </summary>
        public float SalesTaxPercent { get; set; }

        /// <summary>
        /// SalesTaxState
        /// </summary>
        public string SalesTaxState { get; set; }

        #region SalesTaxAmount
        /// <summary>
        /// 或者值
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string Currency { get; set; }
        #endregion

        #endregion


        public int? SellingManagerSalesRecordNumber { get; set; }

        /// <summary>
        /// 是否加急服务
        /// </summary>
        public bool? GetItFast { get; set; }


        public List<ShippingServiceOptionDto> ShippingServiceOptions { get; set; }
    }
}
