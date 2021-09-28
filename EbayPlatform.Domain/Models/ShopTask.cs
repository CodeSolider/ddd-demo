using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Models.Enums;

namespace EbayPlatform.Domain.Models
{
    /// <summary>
    /// 店铺任务
    /// </summary>
    public class ShopTask : Entity<int>
    {
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; private set; }

        /// <summary>
        /// 参数数据 String Json
        /// </summary>
        public string ParamValue { get; private set; }

        /// <summary>
        /// 店铺任务状态
        /// </summary>
        public JobStatusType ShopTaskStatus { get; private set; }


        public ShopTask() { }

        public ShopTask(string shopName, string paramValue)
        {
            this.ShopName = shopName;
            this.ParamValue = paramValue;
            this.ShopTaskStatus = JobStatusType.UnExecute;
        }

        /// <summary>
        /// 更新店铺任务状态
        /// </summary>
        /// <param name="paramValue"></param>
        public void ChangeShopTaskJobStatus(JobStatusType shopTaskStatus)
        {
            this.ShopTaskStatus = shopTaskStatus;
        }

        /// <summary>
        /// 更新店铺任务参数信息
        /// </summary>
        /// <param name="paramValue"></param>
        public void ChangeShopTaskParamValue(string paramValue)
        {
            this.ParamValue = paramValue;
        }
    }
}
