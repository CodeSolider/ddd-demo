using EbayPlatform.Domain.AggregateModel.OrderAggregate;
using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Repository
{
    public class OrderRepository : Repository<Order, long, EbayPlatformDbContext>,
        IOrderRepository, IScopedDependency
    {
        public OrderRepository(EbayPlatformDbContext dbContext)
          : base(dbContext) { }


        /// <summary>
        /// 根据订单ID获取所有订单
        /// </summary>
        /// <param name="orderIDList"></param>
        /// <returns></returns>
        public async Task<List<Order>> GetOrderListByOrderIdsAsync(IEnumerable<string> orderIDList)
        {
            return await this.DbContext.Orders
                             .IgnoreAutoIncludes()
                             .Where(o => orderIDList.Contains(o.OrderID))
                             .ToListAsync()
                             .ConfigureAwait(false);
        }

    }
}
