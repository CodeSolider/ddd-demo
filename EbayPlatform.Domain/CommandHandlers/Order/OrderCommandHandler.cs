using EbayPlatform.Domain.Commands.Order;
using EbayPlatform.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.CommandHandlers.Order
{
    /// <summary>
    /// 订单操作
    /// </summary>
    public class OrderCommandHandler : IRequestHandler<OrderDeleteCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderCommandHandler> _logger;
        public OrderCommandHandler(IOrderRepository orderRepository,
            ILogger<OrderCommandHandler> logger)
        {
            this._orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(OrderDeleteCommand request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository
                                  .GetListAsync(o => request.OrderIDList.Contains(o.OrderID))
                                  .ConfigureAwait(false);

            this._orderRepository.RemoveRange(orderList);
            return await this._orderRepository.UnitOfWork.CommitAsync();
        }
    }
}
