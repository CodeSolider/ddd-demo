using EbayPlatform.Domain.Core.Abstractions;
using System.Collections.Generic;

namespace EbayPlatform.Domain.Models
{
    /// <summary>
    /// 同步任务参数配置表 值对象
    /// </summary>
    public class SyncTaskJobParam : ValueObject
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

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ShopName;
            yield return ParamValue;
        }
    }
}
