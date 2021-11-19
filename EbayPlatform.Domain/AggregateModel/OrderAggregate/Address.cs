using EbayPlatform.Domain.Core.Abstractions;
using System.Collections.Generic;

namespace EbayPlatform.Domain.AggregateModel.OrderAggregate
{
    public class Address : ValueObject
    {
        /// <summary>
        /// eBay 下载的地址ID、唯一
        /// </summary>
        public string AddressID { get; set; }

        /// <summary>
        /// 地址名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 街道
        /// </summary>
        public string Street { get; private set; }

        /// <summary>
        /// 街道1
        /// </summary>
        public string Street1 { get; private set; }

        /// <summary>
        /// 街道2
        /// </summary>
        public string Street2 { get; private set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; private set; }

        /// <summary>
        /// 省/州
        /// </summary>
        public string StateOrProvince { get; private set; }

        /// <summary>
        /// 国家编号
        /// </summary>
        public string Country { get; private set; }

        /// <summary>
        /// 国家名称
        /// </summary>
        public string CountryName { get; private set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; private set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string PostalCode { get; private set; }

        /// <summary>
        /// 所属者
        /// </summary>
        public string AddressOwner { get; private set; }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AddressID;
            yield return Name;
            yield return Street1;
            yield return Street2;
            yield return CityName;
            yield return StateOrProvince;
            yield return Country;
            yield return CountryName;
            yield return Phone;
            yield return PostalCode;
            yield return AddressOwner;
        }


        public Address() { }

        public Address(string addressID, string name, string street, string street1, string street2,
            string cityName, string stateOrProvince, string country, string countryName,
            string phone, string postalCode, string addressOwner)
        {
            this.AddressID = addressID;
            this.Name = name;
            this.Street = street;
            this.Street1 = street1;
            this.Street2 = street2;
            this.CityName = cityName;
            this.StateOrProvince = stateOrProvince;
            this.Country = country;
            this.CountryName = countryName;
            this.Phone = phone;
            this.PostalCode = postalCode;
            this.AddressOwner = addressOwner;
        }
    }
}
