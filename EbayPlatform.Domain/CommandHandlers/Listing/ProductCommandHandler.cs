using EbayPlatform.Domain.Commands.Listing;
using EbayPlatform.Domain.Interfaces;
using MediatR;
using Newtonsoft.Json;
using Serilog.Context;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.CommandHandlers.Listing
{
    public class ProductCommandHandler : IRequestHandler<ProductDeleteCommand, bool>,
         IRequestHandler<ProductCreatedCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public ProductCommandHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        /// <summary>
        /// 根据产品ID删除产品信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        {
            using (LogContext.PushProperty("ProductDeleteCommand", $"{JsonConvert.SerializeObject(request)}"))
            {
                if (!request.ProductDList.Any())
                {
                    return await Task.FromResult(false);
                }
                var productList = await _productRepository
                                        .GetProductListByOrderIdsAsync(request.ProductDList)
                                        .ConfigureAwait(false);

                if (!productList.Any())
                {
                    return false;
                }
                this._productRepository.RemoveRange(productList);
                return await _productRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            }

        }

        /// <summary>
        /// 创建产品信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(ProductCreatedCommand request, CancellationToken cancellationToken)
        {
            using (LogContext.PushProperty("ProductCreatedCommand", $"{JsonConvert.SerializeObject(request)}"))
            {
                if (!request.Products.Any())
                {
                    return await Task.FromResult(false);
                }
                _productRepository.AddRange(request.Products);
                return await _productRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
