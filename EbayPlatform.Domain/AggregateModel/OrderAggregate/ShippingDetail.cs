using EbayPlatform.Domain.Core.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace EbayPlatform.Domain.AggregateModel.OrderAggregate
{
    /// <summary>
    /// 发货明细
    /// </summary>
    public class ShippingDetail : ValueObject
    {
        /// <summary>
        /// 营业税
        /// </summary>
        public SalesTax SalesTax { get; private set; }

        /// <summary>
        /// SellingManagerSalesRecordNumber
        /// </summary>
        public int? SellingManagerSalesRecordNumber { get; private set; }

        /// <summary>
        /// 是否加急服务
        /// </summary>
        public bool? GetItFast { get; private set; }

        /// <summary>
        /// 发货服务选项
        /// </summary>
        public virtual ICollection<ShippingServiceOption> ShippingServiceOptions { get; private set; }


        public ShippingDetail()
        {
            ShippingServiceOptions = new List<ShippingServiceOption>();
        }

        public ShippingDetail(SalesTax salesTax, int? sellingManagerSalesRecordNumber,
            bool? getItFast) : this()
        {
            this.SalesTax = salesTax;
            this.SellingManagerSalesRecordNumber = sellingManagerSalesRecordNumber;
            this.GetItFast = getItFast;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return SalesTax;
            yield return SellingManagerSalesRecordNumber;
            yield return GetItFast;
        }

        /// <summary>
        /// 更新发货服务信息
        /// </summary>
        public void ChangeShippingServiceOption(string shippingService, MoneyValue shippingServiceCost,
            int? shippingServicePriority, bool? expeditedService,
            int? shippingTimeMin, int? shippingTimeMax, ShippingPackage shippingPackage)
        {
            var existsForShippingServiceOption = this.ShippingServiceOptions.SingleOrDefault(o => o.ShippingService.Equals(shippingService));

            if (existsForShippingServiceOption != null)
            {

                existsForShippingServiceOption
                    .SetShippingServiceOption(shippingServiceCost, expeditedService, shippingServicePriority, shippingTimeMin, shippingTimeMax, shippingPackage);
            }
            else
            {
                existsForShippingServiceOption = new ShippingServiceOption(shippingService, shippingServiceCost,
                                                                           shippingServicePriority, expeditedService,
                                                                           shippingTimeMin, shippingTimeMax, shippingPackage);

                this.ShippingServiceOptions.Add(existsForShippingServiceOption);
            }
        }

    }
}
