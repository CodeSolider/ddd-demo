using EbayPlatform.Domain.Core.Abstractions;
using System.Collections.Generic;

namespace EbayPlatform.Domain.AggregateModel.ProductAggregate
{
    /// <summary>
    /// 产品类别
    /// </summary>
    public class ProductCategory : ValueObject
    {
        /// <summary>
        /// 类别ID
        /// </summary>
        public string CategoryID { get; private set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string CategoryName { get; private set; }



        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CategoryID;
            yield return CategoryName;
        }

        public ProductCategory() { }
        public ProductCategory(string categoryID, string categoryName)
        {
            this.CategoryID = categoryID;
            this.CategoryName = categoryName;
        }
    }
}
