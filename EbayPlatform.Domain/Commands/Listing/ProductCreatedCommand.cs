using MediatR;
using System.Collections.Generic;

namespace EbayPlatform.Domain.Commands.Listing
{
    /// <summary>
    /// 创建订单
    /// </summary>
    public class ProductCreatedCommand : IRequest<bool>
    {
        /// <summary>
        /// 添加订单
        /// </summary>
        public List<Models.Listing.Product> Products { get; }

        public ProductCreatedCommand(List<Models.Listing.Product> products)
        {
            this.Products = products;
        }
    }
}
