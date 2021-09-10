using EbayPlatform.Domain.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EbayPlatform.Domain.Models.Orders
{
    /// <summary>
    /// 发货服务选项
    /// </summary>
    public class ShippingServiceOption : ValueObject
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
        public virtual ICollection<ShippingPackage> ShippingPackages { get; private set; }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ShippingService;
            yield return ShippingServiceCost;
            yield return ExpeditedService;
            yield return ShippingTimeMin;
            yield return ShippingTimeMax;
            yield return ShippingPackages;
        }


        public ShippingServiceOption()
        {
            ShippingPackages = new List<ShippingPackage>();
        }

        public ShippingServiceOption(string shippingService, bool? expeditedService,
           int? shippingTimeMin, int? shippingTimeMax)
        {
            this.ShippingService = shippingService;
            this.ExpeditedService = expeditedService;
            this.ShippingTimeMin = shippingTimeMin;
            this.ShippingTimeMax = shippingTimeMax;
        }

        /// <summary>
        /// 更新发货信息
        /// </summary>
        /// <param name="shippingService"></param> 
        /// <param name="expeditedService"></param>
        /// <param name="shippingTimeMin"></param>
        /// <param name="shippingTimeMax"></param>
        public void ChangeShippingServiceOption(string shippingService, bool? expeditedService,
            int? shippingTimeMin, int? shippingTimeMax)
        {
            this.ShippingService = shippingService;
            this.ExpeditedService = expeditedService;
            this.ShippingTimeMin = shippingTimeMin;
            this.ShippingTimeMax = shippingTimeMax;
        }


        /// <summary>
        /// 更新打包信息
        /// </summary>
        /// <param name="storeID"></param>
        /// <param name="shippingTrackingEvent"></param>
        /// <param name="scheduledDeliveryTimeMin"></param>
        /// <param name="scheduledDeliveryTimeMax"></param>
        /// <param name="estimatedDeliveryTimeMin"></param>
        /// <param name="estimatedDeliveryTimeMax"></param>
        public void ChangeShippingPackage(string storeID, string shippingTrackingEvent,
            DateTime? scheduledDeliveryTimeMin, DateTime? scheduledDeliveryTimeMax,
            DateTime? estimatedDeliveryTimeMin, DateTime? estimatedDeliveryTimeMax)
        {
            var existsForshippingPackage = this.ShippingPackages
                                                .SingleOrDefault(o => o.StoreID.Equals(storeID) &&
                                                                      o.ShippingTrackingEvent.Equals(shippingTrackingEvent));

            if (existsForshippingPackage != null)
            {
                existsForshippingPackage
                    .ChangeShippingPackage(scheduledDeliveryTimeMin, scheduledDeliveryTimeMax,
                                            estimatedDeliveryTimeMin, estimatedDeliveryTimeMax);
            }
            else
            {
                this.ShippingPackages.Add(new ShippingPackage(storeID, shippingTrackingEvent,
                                                               scheduledDeliveryTimeMin, scheduledDeliveryTimeMax,
                                                               estimatedDeliveryTimeMin, estimatedDeliveryTimeMax));
            }
        }

    }
}
