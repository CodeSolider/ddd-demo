namespace EbayPlatform.Domain.IntegrationEvents
{
    /// <summary>
    /// 采集数据事件
    /// </summary>
    public class CollectionSyncTaskJobIntegrationEvent
    {
        public CollectionSyncTaskJobIntegrationEvent(string shopName, string paramValue)
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
