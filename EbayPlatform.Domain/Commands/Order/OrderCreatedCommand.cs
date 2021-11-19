using MediatR;
using System.Collections.Generic;

namespace EbayPlatform.Domain.Commands.Order
{
    /// <summary>
    /// 创建订单
    /// </summary>
    public class OrderCreatedCommand : IRequest<bool>
    {
        /// <summary>
        /// 添加订单
        /// </summary>
        public List<AggregateModel.OrderAggregate.Order> Orders { get; }

        public OrderCreatedCommand(List<AggregateModel.OrderAggregate.Order> orders)
        {
            this.Orders = orders;
        }
    }
}
