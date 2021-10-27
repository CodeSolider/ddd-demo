using DotNetCore.CAP;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using EbayPlatform.Application.Dtos;
using EbayPlatform.Application.Dtos.Listing;
using EbayPlatform.Application.Services;
using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.IntegrationEvents;
using EbayPlatform.Infrastructure.Core;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class GetProductListJob : AbstractBaseJob, IJob, ICapSubscribe, IDependency
    {
        private readonly IProductAppService _productAppService;
        public GetProductListJob(ISyncTaskJobAppService syncTaskJobAppService,
           IProductAppService productAppService)
            : base(syncTaskJobAppService)
        {
            _productAppService = productAppService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            string jobName = context.JobDetail.Key.Name;
            return base.PublishQueueAsync(jobName, context.CancellationToken);
        }

        /// <summary>
        /// 处理MQ消息
        /// </summary>
        /// <param name="integrationEvent"></param>
        /// <returns></returns>
        [CapSubscribe(nameof(GetProductListJob))]
        protected override Task ProcessQueueDataAsync(CollectionIntegrationEvent integrationEvent)
        {
            return base.ProcessQueueDataAsync(integrationEvent);
        }


        /// <summary>
        /// 下载前
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected override ApiCall AbstractBaseJob_DownloadingPre(string arg)
        {
            ParamValueToEntityDto paramValueEntityDto = JsonConvert.DeserializeObject<ParamValueToEntityDto>(arg);
            return new eBay.Service.Call.GetSellerListCall
            {
                ApiContext = GetApiContext(paramValueEntityDto.Token),
                Site = SiteCodeType.US,
                Pagination = new PaginationType
                {
                    EntriesPerPage = paramValueEntityDto.PageSize,
                    PageNumber = paramValueEntityDto.PageIndex,
                },
                IncludeVariations = false,
                StartTimeFrom = paramValueEntityDto.FromDate,
                StartTimeTo = (paramValueEntityDto.ToDate - paramValueEntityDto.FromDate).Days > 121 ? paramValueEntityDto.FromDate.AddDays(121) : paramValueEntityDto.ToDate
            };
        }

        /// <summary>
        /// 下载中
        /// </summary>
        /// <param name="apiCall"></param>
        /// <param name="shopName"></param>
        /// <returns></returns>
        protected override Task<ApiResult> DownloaderProvider_Downloading(ApiCall apiCall, string shopName)
        {
            return Task.Run(() =>
            {
                var getSellerListCall = apiCall as eBay.Service.Call.GetSellerListCall;
                bool hasMoreItems = false;

                List<ProductDto> productDtoList = new();
                do
                {
                    getSellerListCall.Execute();
                    hasMoreItems = getSellerListCall.ApiResponse.HasMoreItems.GetValueOrDefault();
                    getSellerListCall.Pagination.PageNumber++;
                    if (getSellerListCall.ItemList.Any())
                    {
                        productDtoList.AddRange(ConvertData(shopName, getSellerListCall.ItemList));
                    }
                } while (hasMoreItems);

                return ApiResult.OK("下载Listing数据成功", new ParamValueToEntityDto<List<ProductDto>>
                {
                    FromDate = getSellerListCall.StartTimeTo,
                    ToDate = DateTime.Now,
                    PageIndex = (getSellerListCall?.Pagination?.PageNumber).GetValueOrDefault(),
                    PageSize = 100,
                    Data = productDtoList
                });
            });
        }

        /// <summary>
        /// 下载完成
        /// </summary>
        /// <param name="apiResult"></param>
        /// <param name="shopName"></param>
        /// <returns></returns>
        protected override async Task DownloaderProvider_DownloadingEnd(ApiResult apiResult, string shopName)
        {
            if (apiResult.Code == 200 && apiResult is ApiResult<ParamValueToEntityDto<List<ProductDto>>> responseData)
            {
                var itemIDList = responseData.Data.Data.Select(o => o.ItemID);
                if (itemIDList.Any())
                {
                    await _productAppService.DeleteProductByIdsAsync(itemIDList).ConfigureAwait(false);
                }

                if (responseData.Data.Data.Any())
                {
                    await _productAppService.AddProductAsync(responseData.Data.Data).ConfigureAwait(false);
                }

                //存储结果集
                await base.ModifySyncTaskJobConfigStatusAsync<List<ProductDto>>(apiResult, nameof(GetProductListJob), shopName).ConfigureAwait(false);
            }
        }

        #region 数据处理

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="shopName"></param>
        /// <param name="itemTypeList"></param>
        /// <returns></returns>
        private List<ProductDto> ConvertData(string shopName, List<ItemType> itemTypeList)
        {
            List<ProductDto> productDtoList = new();
            itemTypeList.ForEach(itemType =>
            {
                ProductDto productDto = new ProductDto
                {
                    ItemID = itemType.ItemID,
                    MSKU = itemType.SKU,
                    ShopName = shopName,
                    Quantity = itemType.Quantity.GetValueOrDefault(),
                    QuantityThreshold = itemType.QuantityThreshold.GetValueOrDefault(),
                    Title = itemType.Title,
                    Description = itemType.Description,
                    HitCount = itemType.HitCount.GetValueOrDefault(),
                    Location = itemType.Location,
                    PaymentMethods = string.Join(",", itemType.PaymentMethods.Select(o => o.ToString()))
                };

                if (itemType.Site.HasValue)
                {
                    productDto.SiteCode = Convert.ToString(itemType.Site.Value);
                }

                if (itemType.StartPrice != null)
                {
                    productDto.StartPriceValue = (decimal)itemType.StartPrice.Value;
                    productDto.StartPriceCurrency = Convert.ToString(itemType.StartPrice.currencyID);
                }

                if (itemType.BuyerGuaranteePrice != null)
                {
                    productDto.BuyerGuaranteePriceValue = (decimal)itemType.BuyerGuaranteePrice.Value;
                    productDto.BuyerGuaranteePriceCurrency = Convert.ToString(itemType.BuyerGuaranteePrice.currencyID);
                }

                if (itemType.BuyItNowPrice != null)
                {
                    productDto.BuyItNowPriceValue = (decimal)itemType.BuyItNowPrice.Value;
                    productDto.BuyItNowPriceCurrency = Convert.ToString(itemType.BuyItNowPrice.currencyID);
                }

                if (itemType.ReservePrice != null)
                {
                    productDto.ReservePriceValue = (decimal)itemType.ReservePrice.Value;
                    productDto.ReservePriceCurrency = Convert.ToString(itemType.ReservePrice.currencyID);
                }

                if (itemType.QuantityAvailableHint.HasValue)
                {
                    productDto.QuantityAvailableHint = Convert.ToString(itemType.QuantityAvailableHint.Value);

                }

                if (itemType.Country.HasValue)
                {
                    productDto.Country = Convert.ToString(itemType.Country.Value);
                }

                if (itemType.Currency.HasValue)
                {
                    productDto.CurrencyCode = Convert.ToString(itemType.Currency.Value);
                }

                if (itemType.HitCounter.HasValue)
                {
                    productDto.HitCounter = Convert.ToString(itemType.HitCounter.Value);
                }

                if (itemType.InventoryTrackingMethod.HasValue)
                {
                    productDto.InventoryTrackingMethod = Convert.ToString(itemType.InventoryTrackingMethod.Value);
                }

                if (itemType.ListingType.HasValue)
                {
                    productDto.ListingType = Convert.ToString(itemType.ListingType.Value);
                }

                if (itemType.ListingSubtype2.HasValue)
                {
                    productDto.ListingSubType = Convert.ToString(itemType.ListingSubtype2.Value);
                }

                if (itemType.FreeAddedCategory != null)
                {
                    productDto.FreeAddedCategoryCategoryID = itemType.FreeAddedCategory.CategoryID;
                    productDto.FreeAddedCategoryCategoryName = itemType.FreeAddedCategory.CategoryName;
                }

                if (itemType.PrimaryCategory != null)
                {
                    productDto.PrimaryCategoryID = itemType.PrimaryCategory.CategoryID;
                    productDto.PrimaryCategoryName = itemType.PrimaryCategory.CategoryName;
                }

                if (itemType.SecondaryCategory != null)
                {
                    productDto.SecondaryCategoryID = itemType.SecondaryCategory.CategoryID;
                    productDto.SecondaryCategoryName = itemType.SecondaryCategory.CategoryName;
                }

                if (itemType.PrivateListing.HasValue)
                {
                    productDto.PrivateListing = itemType.PrivateListing.GetValueOrDefault();
                }

                if (itemType.ReviseStatus != null)
                {
                    productDto.ItemRevised = itemType.ReviseStatus.ItemRevised.GetValueOrDefault();
                }

                if (itemType.SellingStatus != null)
                {
                    if (itemType.SellingStatus.ListingStatus.HasValue)
                    {
                        productDto.ListingStatus = Convert.ToString(itemType.SellingStatus.ListingStatus);
                    }
                    productDto.QuantitySold = itemType.SellingStatus.QuantitySold.GetValueOrDefault();
                    if (itemType.SellingStatus.ConvertedCurrentPrice != null)
                    {
                        productDto.ConvertedCurrentPriceValue = (decimal)itemType.SellingStatus.ConvertedCurrentPrice.Value;
                        productDto.ConvertedCurrentPriceCurrency = Convert.ToString(itemType.SellingStatus.ConvertedCurrentPrice.currencyID);
                    }
                    if (itemType.SellingStatus.CurrentPrice != null)
                    {
                        productDto.CurrentPriceValue = (decimal)itemType.SellingStatus.CurrentPrice.Value;
                        productDto.CurrentPriceCurrency = Convert.ToString(itemType.SellingStatus.CurrentPrice.currencyID);
                    }

                    if (itemType.SellingStatus.PromotionalSaleDetails != null)
                    {
                        productDto.StartTime = itemType.SellingStatus.PromotionalSaleDetails.StartTime;
                        productDto.EndTime = itemType.SellingStatus.PromotionalSaleDetails.EndTime;
                    }
                    productDto.EndTime = productDto.EndTime;
                    productDto.AdminEnded = itemType.SellingStatus.AdminEnded.GetValueOrDefault();
                }
            });
            return productDtoList;
        }
        #endregion

    }
}
