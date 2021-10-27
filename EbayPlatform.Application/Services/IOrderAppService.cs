using EbayPlatform.Application.Dtos.Orders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Services
{
    /// <summary>
    /// 订单服务
    /// </summary>
    public interface IOrderAppService
    {
        /// <summary>
        /// 根据订单ID删除订单信息
        /// </summary>
        /// <param name="orderIdList"></param>
        /// <returns></returns>
        Task<bool> DeleteOrderByIdsAsync(IEnumerable<string> orderIdList, CancellationToken cancellationToken = default);

        /// <summary>
        /// 添加订单数据
        /// </summary>
        /// <param name="orderDtos"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AddOrderAsync(List<OrderDto> orderDtos, CancellationToken cancellationToken = default);
    }
}
