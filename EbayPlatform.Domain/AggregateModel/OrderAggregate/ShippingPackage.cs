using EbayPlatform.Domain.Core.Abstractions;
using System;
using System.Collections.Generic;

namespace EbayPlatform.Domain.AggregateModel.OrderAggregate
{
    public class ShippingPackage : ValueObject
    {
        /// <summary>
        /// StoreID
        /// </summary>
        public string StoreID { get; private set; }

        /// <summary>
        /// ShippingTrackingEvent
        /// </summary>
        public string ShippingTrackingEvent { get; private set; }

        /// <summary>
        /// ScheduledDeliveryTimeMin
        /// </summary>
        public DateTime? ScheduledDeliveryTimeMin { get; private set; }

        /// <summary>
        /// ScheduledDeliveryTimeMax
        /// </summary>
        public DateTime? ScheduledDeliveryTimeMax { get; private set; }


        /// <summary>
        /// ActualDeliveryTime
        /// </summary>
        public DateTime? ActualDeliveryTime { get; private set; }

        /// <summary>
        /// 预计发货时间 Min
        /// </summary>
        public DateTime? EstimatedDeliveryTimeMin { get; private set; }

        /// <summary>
        /// 预计发货时间 Max
        /// </summary>
        public DateTime? EstimatedDeliveryTimeMax { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StoreID;
            yield return ShippingTrackingEvent;
            yield return ScheduledDeliveryTimeMin;
            yield return ScheduledDeliveryTimeMax;
            yield return ActualDeliveryTime;
            yield return EstimatedDeliveryTimeMin;
            yield return EstimatedDeliveryTimeMax;
        }


        public ShippingPackage() { }

        public ShippingPackage(string storeID, string shippingTrackingEvent,
            DateTime? scheduledDeliveryTimeMin, DateTime? scheduledDeliveryTimeMax,
            DateTime? actualDeliveryTime, DateTime? estimatedDeliveryTimeMin, 
            DateTime? estimatedDeliveryTimeMax)
        {
            this.StoreID = storeID;
            this.ShippingTrackingEvent = shippingTrackingEvent;
            this.ScheduledDeliveryTimeMin = scheduledDeliveryTimeMin;
            this.ScheduledDeliveryTimeMax = scheduledDeliveryTimeMax;
            this.ActualDeliveryTime = actualDeliveryTime;
            this.EstimatedDeliveryTimeMin = estimatedDeliveryTimeMin;
            this.EstimatedDeliveryTimeMax = estimatedDeliveryTimeMax;
        }
    }
}
