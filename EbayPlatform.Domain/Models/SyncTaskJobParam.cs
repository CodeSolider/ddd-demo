using EbayPlatform.Domain.Core.Abstractions;

namespace EbayPlatform.Domain.Models
{
    /// <summary>
    /// 同步任务参数配置表 值对象
    /// </summary>
    public class SyncTaskJobParam : Entity<int>
    {
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; private set; }

        /// <summary>
        /// 参数数据 String Json
        /// </summary>
        public string ParamValue { get; private set; }


        public SyncTaskJobParam() { }

        public SyncTaskJobParam(string shopName, string paramValue)
        {
            this.ShopName = shopName;
            this.ParamValue = paramValue;
        }

        /// <summary>
        /// 更新参数值
        /// </summary>
        /// <param name="paramValue"></param>
        public void ChangeParamValue(string paramValue)
        {
            this.ParamValue = paramValue;
        }

    }
}
