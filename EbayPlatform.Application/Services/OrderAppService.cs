using EbayPlatform.Domain.Commands.Order;
using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Infrastructure.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Services
{
    /// <summary>
    /// 订单服务
    /// </summary>
    public class OrderAppService : IOrderAppService, IDependency
    {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _orderRepository;
        public OrderAppService(IMediator mediator, IOrderRepository orderRepository)
        {
            _mediator = mediator;
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// 根据订单ID获取订单
        /// </summary>
        /// <param name="orderIdList"></param>
        /// <returns></returns>
        public async Task<bool> GetOrderListByOrderIdsAsync(IEnumerable<string> orderIdList)
        {
            return await _mediator.Send(new OrderDeleteCommand(orderIdList)).ConfigureAwait(false);
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
