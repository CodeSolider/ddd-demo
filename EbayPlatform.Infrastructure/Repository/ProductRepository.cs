using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Domain.Models.Listing;
using EbayPlatform.Infrastructure.Context;
using EbayPlatform.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Repository
{
    /// <summary>
    /// Product 仓储
    /// </summary>
    public class ProductRepository : Repository<Product, long, EbayPlatformDbContext>,
        IProductRepository, IDependency
    {
        public ProductRepository(EbayPlatformDbContext dbContext)
        : base(dbContext) { }

        /// <summary>
        /// 根据产品ID获取所有产品
        /// </summary>
        /// <param name="productIDList"></param>
        /// <returns></returns>
        public async Task<List<Product>> GetProductListByOrderIdsAsync(IEnumerable<string> productIDList)
        {
            return await this.DbContext.Products
                             .Where(o => productIDList.Contains(o.ItemID))
                             .ToListAsync()
                             .ConfigureAwait(false);
        }

    }
}
