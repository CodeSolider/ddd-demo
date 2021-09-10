using MediatR;
using System.Collections.Generic;

namespace EbayPlatform.Domain.Commands.Order
{
    public class OrderDeleteCommand : IRequest<bool>
    {
        public IEnumerable<string> OrderIDList { get; }

        public OrderDeleteCommand(IEnumerable<string> orderIDList)
        {
            this.OrderIDList = orderIDList;
        }
    }
}
