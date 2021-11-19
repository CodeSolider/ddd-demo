using EbayPlatform.Application.Dtos.Listing;
using EbayPlatform.Domain.Commands.Listing;
using EbayPlatform.Domain.Core.Abstractions;
using MediatR; 
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Services
{
    public class ProductAppService : IProductAppService, IScopedDependency
    {
        private readonly IMediator _mediator;
        public ProductAppService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<bool> DeleteProductByIdsAsync(IEnumerable<string> productIdList, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new ProductDeleteCommand(productIdList), cancellationToken);
        }

        /// <summary>
        /// 添加产品信息
        /// </summary>
        /// <param name="productDtos"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> AddProductAsync(List<ProductDto> productDtos, CancellationToken cancellationToken = default)
        {
            List<Domain.AggregateModel.ProductAggregate.Product> productList = new();

            productDtos.ForEach(productDtoItem =>
            {
                Domain.AggregateModel.ProductAggregate.PromotionalSaleDetail promotionalSaleDetailItem = new(productDtoItem.StartTime, new Domain.AggregateModel.MoneyValue(productDtoItem.OriginalPriceValue, productDtoItem.OriginalPriceCurrency),
                                                                                            productDtoItem.EndTime);


                Domain.AggregateModel.ProductAggregate.SellingStatus sellingStatusItem = new(new Domain.AggregateModel.MoneyValue(productDtoItem.ConvertedCurrentPriceValue, productDtoItem.ConvertedCurrentPriceCurrency),
                                                                            new Domain.AggregateModel.MoneyValue(productDtoItem.CurrentPriceValue, productDtoItem.CurrentPriceCurrency), productDtoItem.ListingStatus,
                                                                            productDtoItem.QuantitySold, productDtoItem.AdminEnded, promotionalSaleDetailItem);


                Domain.AggregateModel.ProductAggregate.Product productItem = new(productDtoItem.ItemID, productDtoItem.MSKU, productDtoItem.ShopName, productDtoItem.SiteCode,
                                                                new Domain.AggregateModel.MoneyValue(productDtoItem.StartPriceValue, productDtoItem.StartPriceCurrency),
                                                                new Domain.AggregateModel.MoneyValue(productDtoItem.BuyerGuaranteePriceValue, productDtoItem.BuyerGuaranteePriceCurrency),
                                                                new Domain.AggregateModel.MoneyValue(productDtoItem.BuyItNowPriceValue, productDtoItem.BuyItNowPriceCurrency),
                                                                new Domain.AggregateModel.MoneyValue(productDtoItem.ReservePriceValue, productDtoItem.ReservePriceCurrency),
                                                                productDtoItem.Quantity, productDtoItem.QuantityAvailableHint, productDtoItem.QuantityThreshold,
                                                                productDtoItem.Country, productDtoItem.CurrencyCode, productDtoItem.Title, productDtoItem.Description,
                                                                productDtoItem.HitCount, productDtoItem.HitCounter, productDtoItem.InventoryTrackingMethod,
                                                                productDtoItem.ListingType, productDtoItem.ListingSubType, productDtoItem.Location, productDtoItem.PaymentMethods,
                                                                sellingStatusItem, new Domain.AggregateModel.ProductAggregate.ProductCategory(productDtoItem.FreeAddedCategoryCategoryID, productDtoItem.FreeAddedCategoryCategoryName),
                                                                new Domain.AggregateModel.ProductAggregate.ProductCategory(productDtoItem.PrimaryCategoryID, productDtoItem.PrimaryCategoryName),
                                                                new Domain.AggregateModel.ProductAggregate.ProductCategory(productDtoItem.SecondaryCategoryID, productDtoItem.SecondaryCategoryName),
                                                                productDtoItem.PrivateListing, productDtoItem.ItemRevised);
                productList.Add(productItem);

            });
            return _mediator.Send(new ProductCreatedCommand(productList), cancellationToken);
        }
    }
}
