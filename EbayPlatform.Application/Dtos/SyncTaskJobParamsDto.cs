namespace EbayPlatform.Application.Dtos
{
    public class SyncTaskJobParamsDto
    {
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 参数数据 String Json
        /// </summary>
        public string ParamValue { get; set; }
    }
}
