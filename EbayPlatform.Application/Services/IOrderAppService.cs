using System.Collections.Generic;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Services
{
    /// <summary>
    /// 订单服务
    /// </summary>
    public interface IOrderAppService
    {
        /// <summary>
        /// 根据订单ID获取订单
        /// </summary>
        /// <param name="orderIdList"></param>
        /// <returns></returns>
        Task<bool> GetOrderListByOrderIdsAsync(IEnumerable<string> orderIdList);
    }
}
