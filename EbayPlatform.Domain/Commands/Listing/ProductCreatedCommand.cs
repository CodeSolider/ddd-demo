using EbayPlatform.Domain.AggregateModel.ProductAggregate;
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
        public List<Product> Products { get; }

        public ProductCreatedCommand(List<Product> products)
        {
            this.Products = products;
        }
    }
}
