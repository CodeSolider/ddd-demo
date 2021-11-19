using EbayPlatform.Domain.Core.Abstractions; 

namespace EbayPlatform.Domain.Events.SyncTaskJobConfig
{
    public class CollectionSyncTaskJobDomainEvent : IDomainEvent
    {
        public CollectionSyncTaskJobDomainEvent(string shopName, string paramValue)
        {
            this.ShopName = shopName;
            this.ParamValue = paramValue;
        }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; }
    }
}
