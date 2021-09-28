namespace EbayPlatform.Application.Dtos.Orders
{
    public class AddressDto
    {
        /// <summary>
        /// eBay 下载的地址ID、唯一
        /// </summary>
        public string AddressID { get; set; }

        /// <summary>
        /// 地址名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 街道
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// 街道1
        /// </summary>
        public string Street1 { get; set; }

        /// <summary>
        /// 街道2
        /// </summary>
        public string Street2 { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 省/州
        /// </summary>
        public string StateOrProvince { get; set; }

        /// <summary>
        /// 国家编号
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 国家名称
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// 所属者
        /// </summary>
        public string AddressOwner { get; set; }
    }
}
