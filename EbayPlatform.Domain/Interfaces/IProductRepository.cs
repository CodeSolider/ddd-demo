using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Models.Listing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product, long>
    {
        /// <summary>
        /// 根据产品ID获取所有产品
        /// </summary>
        /// <param name="productIDList"></param>
        /// <returns></returns>
        Task<List<Product>> GetProductListByOrderIdsAsync(IEnumerable<string> productIDList);
    }
}
