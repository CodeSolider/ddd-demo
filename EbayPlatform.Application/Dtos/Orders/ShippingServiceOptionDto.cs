using System.Collections.Generic;

namespace EbayPlatform.Application.Dtos.Orders
{
    public class ShippingServiceOptionDto
    {
        public ShippingServiceOptionDto()
        {
            ShippingPackages = new List<ShippingPackageDto>();
        }

        /// <summary>
        /// 发货服务
        /// </summary>
        public string ShippingService { get; set; }

        #region ShippingServiceCost
        /// <summary>
        /// 或者值
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string Currency { get; set; }
        #endregion 

        /// <summary>
        /// 发货服务优先级
        /// </summary>
        public int? ShippingServicePriority { get; set; }

        /// <summary>
        /// 是否加急服务
        /// </summary>
        public bool? ExpeditedService { get; set; }

        /// <summary>
        /// 装运时间 Min
        /// </summary>
        public int? ShippingTimeMin { get; set; }

        /// <summary>
        /// 装运时间 Max
        /// </summary>
        public int? ShippingTimeMax { get; set; }


        public List<ShippingPackageDto> ShippingPackages { get; set; }
    }
}
