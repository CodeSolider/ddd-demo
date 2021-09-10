using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Models.Orders; 

namespace EbayPlatform.Domain.Interfaces
{
    /// <summary>
    /// 获取订单
    /// </summary>
    public interface IOrderRepository : IRepository<Order, long>
    {
    }
}
