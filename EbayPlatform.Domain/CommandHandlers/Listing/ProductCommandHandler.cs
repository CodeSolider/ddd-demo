using EbayPlatform.Domain.Commands.Listing;
using EbayPlatform.Domain.Interfaces;
using MediatR;
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

        /// <summary>
        /// 创建产品信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> Handle(ProductCreatedCommand request, CancellationToken cancellationToken)
        {
            if (!request.Products.Any())
            {
                return Task.FromResult(false);
            }
            _productRepository.AddRange(request.Products);
            return _productRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        /// <summary>
        /// 手动释放
        /// </summary>
        public void Dispose()
        {
            _productRepository.Dispose();
        }
    }
}
