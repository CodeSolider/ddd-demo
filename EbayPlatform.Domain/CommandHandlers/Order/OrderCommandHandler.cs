using EbayPlatform.Domain.Commands.Order;
using EbayPlatform.Domain.Interfaces;
using MediatR;
using Newtonsoft.Json;
using Serilog.Context;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.CommandHandlers.Order
{
    /// <summary>
    /// 订单操作
    /// </summary>
    public class OrderCommandHandler : IRequestHandler<OrderDeleteCommand, bool>,
        IRequestHandler<OrderCreatedCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        public OrderCommandHandler(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }

        /// <summary>
        /// 根据订单ID删除订单数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(OrderDeleteCommand request, CancellationToken cancellationToken)
        {
            using (LogContext.PushProperty("OrderDeleteCommand", $"{JsonConvert.SerializeObject(request)}"))
            {
                if (!request.OrderIDList.Any())
                {
                    return false;
                }
                var orderList = await _orderRepository
                                      .GetOrderListByOrderIdsAsync(request.OrderIDList)
                                      .ConfigureAwait(false);

                if (!orderList.Any())
                {
                    return false;
                }
                this._orderRepository.RemoveRange(orderList);
                return await _orderRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            }
        }


        /// <summary>
        /// 批量创建订单数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(OrderCreatedCommand request, CancellationToken cancellationToken)
        {
            using (LogContext.PushProperty("OrderCreatedCommand", $"{JsonConvert.SerializeObject(request)}"))
            {
                if (request.Orders.Any())
                {
                    _orderRepository.AddRange(request.Orders);
                }
                return await _orderRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
