using System;

namespace EbayPlatform.Application.Dtos.Orders
{
    public class ShippingPackageDto
    {
        /// <summary>
        /// StoreID
        /// </summary>
        public string StoreID { get; set; }

        /// <summary>
        /// ShippingTrackingEvent
        /// </summary>
        public string ShippingTrackingEvent { get; set; }

        /// <summary>
        /// ScheduledDeliveryTimeMin
        /// </summary>
        public DateTime? ScheduledDeliveryTimeMin { get; set; }

        /// <summary>
        /// ScheduledDeliveryTimeMax
        /// </summary>
        public DateTime? ScheduledDeliveryTimeMax { get; set; }


        /// <summary>
        /// ActualDeliveryTime
        /// </summary>
        public DateTime? ActualDeliveryTime { get; set; }

        /// <summary>
        /// 预计发货时间 Min
        /// </summary>
        public DateTime? EstimatedDeliveryTimeMin { get; set; }

        /// <summary>
        /// 预计发货时间 Max
        /// </summary>
        public DateTime? EstimatedDeliveryTimeMax { get; set; }
    }
}
