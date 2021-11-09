namespace EbayPlatform.Application.Dtos.ScticooErp
{
    /// <summary>
    /// Erp 店铺信息
    /// </summary>
    public class PlatformShopAccountDto
    {
        public PlatformShopAccount PlatformShopAccount { get; set; }
    }

    public class PlatformShopAccount
    {
        /// <summary>
        /// 店铺别名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }
    }
}
