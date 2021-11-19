using EbayPlatform.Domain.Core.Abstractions;
using System.Collections.Generic;

namespace EbayPlatform.Domain.AggregateModel.OrderAggregate
{
    /// <summary>
    /// 发货服务选项
    /// </summary>
    public class ShippingServiceOption : Entity<long>
    {
        /// <summary>
        /// 发货服务
        /// </summary>
        public string ShippingService { get; private set; }

        /// <summary>
        /// 运输服务费
        /// </summary>
        public MoneyValue ShippingServiceCost { get; private set; }

        /// <summary>
        /// 发货服务优先级
        /// </summary>
        public int? ShippingServicePriority { get; private set; }

        /// <summary>
        /// 是否加急服务
        /// </summary>
        public bool? ExpeditedService { get; private set; }

        /// <summary>
        /// 装运时间 Min
        /// </summary>
        public int? ShippingTimeMin { get; private set; }

        /// <summary>
        /// 装运时间 Max
        /// </summary>
        public int? ShippingTimeMax { get; private set; }

        /// <summary>
        /// 发货打包信息
        /// </summary>
        public ShippingPackage ShippingPackage { get; private set; }

        protected ShippingServiceOption() { }
        public ShippingServiceOption(string shippingService, MoneyValue shippingServiceCost,
            int? shippingServicePriority, bool? expeditedService,
            int? shippingTimeMin, int? shippingTimeMax, ShippingPackage shippingPackage)
        {
            this.ShippingService = shippingService;
            this.ShippingServiceCost = shippingServiceCost;
            this.ShippingServicePriority = shippingServicePriority;
            this.ExpeditedService = expeditedService;
            this.ShippingTimeMin = shippingTimeMin;
            this.ShippingTimeMax = shippingTimeMax;
            this.ShippingPackage = shippingPackage;
        }

        /// <summary>
        /// 设置发货服务值
        /// </summary>
        /// <param name="shippingServiceCost"></param>
        /// <param name="shippingServicePriority"></param>
        /// <param name="expeditedService"></param>
        /// <param name="shippingTimeMin"></param>
        /// <param name="shippingTimeMax"></param>
        public void SetShippingServiceOption(MoneyValue shippingServiceCost, bool? expeditedService,
            int? shippingServicePriority, int? shippingTimeMin, int? shippingTimeMax,
            ShippingPackage shippingPackage)
        {
            this.ShippingServiceCost = shippingServiceCost;
            this.ExpeditedService = expeditedService;
            this.ShippingServicePriority = shippingServicePriority;
            this.ShippingTimeMin = shippingTimeMin;
            this.ShippingTimeMax = shippingTimeMax;
            this.ShippingPackage = shippingPackage;
        }
    }
}
