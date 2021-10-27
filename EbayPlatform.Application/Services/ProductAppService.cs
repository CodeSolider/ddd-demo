using EbayPlatform.Application.Dtos.Listing;
using EbayPlatform.Domain.Commands.Listing;
using EbayPlatform.Domain.Core.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Services
{
    public class ProductAppService : IProductAppService, IDependency
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
            List<Domain.Models.Listing.Product> productList = new();

            productDtos.ForEach(productDtoItem =>
            {
                Domain.Models.Listing.PromotionalSaleDetail promotionalSaleDetailItem = new(productDtoItem.StartTime, new Domain.Models.MoneyValue(productDtoItem.OriginalPriceValue, productDtoItem.OriginalPriceCurrency),
                                                                                            productDtoItem.EndTime);


                Domain.Models.Listing.SellingStatus sellingStatusItem = new(new Domain.Models.MoneyValue(productDtoItem.ConvertedCurrentPriceValue, productDtoItem.ConvertedCurrentPriceCurrency),
                                                                            new Domain.Models.MoneyValue(productDtoItem.CurrentPriceValue, productDtoItem.CurrentPriceCurrency), productDtoItem.ListingStatus,
                                                                            productDtoItem.QuantitySold, productDtoItem.AdminEnded, promotionalSaleDetailItem);


                Domain.Models.Listing.Product productItem = new(productDtoItem.ItemID, productDtoItem.MSKU, productDtoItem.ShopName, productDtoItem.SiteCode,
                                                                new Domain.Models.MoneyValue(productDtoItem.StartPriceValue, productDtoItem.StartPriceCurrency),
                                                                new Domain.Models.MoneyValue(productDtoItem.BuyerGuaranteePriceValue, productDtoItem.BuyerGuaranteePriceCurrency),
                                                                new Domain.Models.MoneyValue(productDtoItem.BuyItNowPriceValue, productDtoItem.BuyItNowPriceCurrency),
                                                                new Domain.Models.MoneyValue(productDtoItem.ReservePriceValue, productDtoItem.ReservePriceCurrency),
                                                                productDtoItem.Quantity, productDtoItem.QuantityAvailableHint, productDtoItem.QuantityThreshold,
                                                                productDtoItem.Country, productDtoItem.CurrencyCode, productDtoItem.Title, productDtoItem.Description,
                                                                productDtoItem.HitCount, productDtoItem.HitCounter, productDtoItem.InventoryTrackingMethod,
                                                                productDtoItem.ListingType, productDtoItem.ListingSubType, productDtoItem.Location, productDtoItem.PaymentMethods,
                                                                sellingStatusItem, new Domain.Models.Listing.ProductCategory(productDtoItem.FreeAddedCategoryCategoryID, productDtoItem.FreeAddedCategoryCategoryName),
                                                                new Domain.Models.Listing.ProductCategory(productDtoItem.PrimaryCategoryID, productDtoItem.PrimaryCategoryName),
                                                                new Domain.Models.Listing.ProductCategory(productDtoItem.SecondaryCategoryID, productDtoItem.SecondaryCategoryName),
                                                                productDtoItem.PrivateListing, productDtoItem.ItemRevised);
                productList.Add(productItem);

            });
            return _mediator.Send(new ProductCreatedCommand(productList), cancellationToken);
        }

#pragma warning disable CA1816 // Dispose 方法应调用 SuppressFinalize
        public void Dispose() => GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose 方法应调用 SuppressFinalize
    }
}
