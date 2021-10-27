using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Domain.Models.Orders;
using EbayPlatform.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Repository
{
    public class OrderRepository : Repository<Order, long, EbayPlatformDbContext>,
        IOrderRepository, IDependency
    {
        public OrderRepository(EbayPlatformDbContext dbContext)
          : base(dbContext) { }


        /// <summary>
        /// 根据订单ID获取所有订单
        /// </summary>
        /// <param name="orderIDList"></param>
        /// <returns></returns>
        public Task<List<Order>> GetOrderListByOrderIdsAsync(IEnumerable<string> orderIDList)
        {
            return this.DbContext.Orders
                       .Where(o => orderIDList.Contains(o.OrderID))
                       .ToListAsync();
        }

    }
}
