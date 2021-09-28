using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Models.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.Interfaces
{
    /// <summary>
    /// 获取订单
    /// </summary>
    public interface IOrderRepository : IRepository<Order, long>
    {
        /// <summary>
        /// 根据订单ID获取所有订单
        /// </summary>
        /// <param name="orderIDList"></param>
        /// <returns></returns>
        Task<List<Order>> GetOrderListByOrderIdsAsync(IEnumerable<string> orderIDList);
    }
}
