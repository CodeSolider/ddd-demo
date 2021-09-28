using MediatR;
using System.Collections.Generic; 

namespace EbayPlatform.Domain.Commands.Listing
{
    public class ProductDeleteCommand : IRequest<bool>
    {
        public IEnumerable<string> ProductDList { get; }

        public ProductDeleteCommand(IEnumerable<string> productDList)
        {
            ProductDList = productDList;
        }
    }
}
