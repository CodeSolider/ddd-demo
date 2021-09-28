using EbayPlatform.Application.Dtos.Listing;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Services
{
    public interface IProductAppService
    {
        /// <summary>
        /// 根据产品ID获取产品
        /// </summary>
        /// <param name="productIdList"></param>
        /// <returns></returns>
        Task<bool> DeleteProductByIdsAsync(IEnumerable<string> productIdList, CancellationToken cancellationToken = default);

        /// <summary>
        /// 添加订单数据
        /// </summary>
        /// <param name="productDtos"></param>
        /// <returns></returns>
        Task<bool> AddProductAsync(List<ProductDto> productDtos, CancellationToken cancellationToken = default);
    }
}
