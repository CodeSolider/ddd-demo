using EbayPlatform.Domain.Commands.Order;
using EbayPlatform.Domain.Interfaces;
using MediatR;
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
            if (!request.OrderIDList.Any())
            {
                return await Task.FromResult(false);
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


        /// <summary>
        /// 批量订单数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> Handle(OrderCreatedCommand request, CancellationToken cancellationToken)
        {
            if (!request.Orders.Any())
            {
                return Task.FromResult(false);
            }
            _orderRepository.AddRange(request.Orders);
            return _orderRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        /// <summary>
        /// 手动释放
        /// </summary>
        public void Dispose()
        {
            _orderRepository.Dispose();
        }
    }
}
