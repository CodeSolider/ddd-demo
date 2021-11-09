using DotNetCore.CAP;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using EbayPlatform.Application.Dtos;
using EbayPlatform.Application.Dtos.Accounts;
using EbayPlatform.Application.Services;
using EbayPlatform.Domain.IntegrationEvents;
using EbayPlatform.Infrastructure.Core;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using EbayPlatform.Application.Dtos.Orders;
using EbayPlatform.Infrastructure.Core.Engines;
using EbayPlatform.Domain.Core.Abstractions;

namespace EbayPlatform.Application.Quartz.Jobs
{
    /// <summary>
    /// 获取账单信息
    /// </summary>
    [DisallowConcurrentExecution]
    public class GetAccountJob : AbstractBaseJob, IJob, ICapSubscribe, IDependency
    {
        private readonly IAccountAppService _accountAppService;

        public GetAccountJob()
        {
            _accountAppService = EngineContext.Current.Resolve<IAccountAppService>();
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
        [CapSubscribe(nameof(GetAccountJob))]
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
            return new GetAccountCall
            {
                ApiContext = base.GetApiContext(paramValueEntityDto.Token),
                Site = SiteCodeType.US,
                Pagination = new PaginationType
                {
                    EntriesPerPage = paramValueEntityDto.PageSize,
                    PageNumber = paramValueEntityDto.PageIndex,
                },
                DetailLevelList = new List<DetailLevelCodeType>
                {
                    DetailLevelCodeType.ReturnAll
                },
                AccountHistorySelection = AccountHistorySelectionCodeType.SpecifiedInvoice,
                BeginDate = paramValueEntityDto.FromDate,
                EndDate = ((paramValueEntityDto.ToDate.Year - paramValueEntityDto.FromDate.Year) * 12 + paramValueEntityDto.ToDate.Month - paramValueEntityDto.FromDate.Month) > 4 ? paramValueEntityDto.FromDate.AddMonths(4) : paramValueEntityDto.ToDate //只返回过去四个月内发布的记录
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
                var getAccountCall = apiCall as GetAccountCall;
                bool hasMoreEntries = false;
                List<AccountDto> accountDtoList = new();
                do
                {
                    getAccountCall.Execute();
                    hasMoreEntries = getAccountCall.ApiResponse.HasMoreEntries.GetValueOrDefault();
                    getAccountCall.Pagination.PageNumber++;
                    if (getAccountCall.ApiResponse != null && getAccountCall.ApiResponse.AccountSummary != null)
                    {
                        accountDtoList.Add(ConverData(shopName, getAccountCall.ApiResponse));
                    }
                } while (hasMoreEntries);

                if (!accountDtoList.Any())
                {
                    return ApiResult.Fail("暂无可下载的账单数据");
                }

                return ApiResult.OK("下载账单数据成功", new ParamValueToEntityDto<List<AccountDto>>
                {
                    FromDate = getAccountCall.BeginDate,
                    ToDate = DateTime.Now,
                    PageIndex = (getAccountCall?.Pagination?.PageNumber).GetValueOrDefault(),
                    PageSize = 100,
                    Data = accountDtoList
                });
            });
        }

        /// <summary>
        /// 下载后
        /// </summary>
        /// <param name="apiResult"></param>
        /// <param name="shopName"></param>
        /// <returns></returns>
        protected override async Task DownloaderProvider_DownloadingEnd(ApiResult apiResult, string shopName)
        {
            if (apiResult.Code == 200 && apiResult is ApiResult<ParamValueToEntityDto<List<AccountDto>>> responseData)
            {
                var accountIDList = responseData.Data.Data.Select(o => o.AccountID);
                if (accountIDList.Any())
                {
                    await _accountAppService.DeleteAccountIdsAsync(accountIDList).ConfigureAwait(false);
                }

                if (responseData.Data.Data.Any())
                {
                    await _accountAppService.AddAccountAsync(responseData.Data.Data).ConfigureAwait(false);
                }

                //存储结果集
                await base.ModifySyncTaskJobConfigStatusAsync<List<OrderDto>>(apiResult, nameof(GetAllOrderListJob), shopName).ConfigureAwait(false);
            }
        }

        #region 数据转换

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="shopName"></param>
        /// <param name="getAccountResponseType"></param>
        /// <returns></returns>
        private AccountDto ConverData(string shopName, GetAccountResponseType getAccountResponseType)
        {
            var accountDto = new AccountDto
            {
                AccountID = getAccountResponseType.AccountID,
                ShopName = shopName,
                CurrencyCode = Convert.ToString(getAccountResponseType.Currency),
                AccountState = Convert.ToString(getAccountResponseType.AccountSummary.AccountState)
            };

            accountDto.InvoicePaymentValue = 0M;
            if (getAccountResponseType.AccountSummary.InvoicePayment != null)
            {
                accountDto.InvoicePaymentValue = (decimal)getAccountResponseType.AccountSummary.InvoicePayment.Value;
                accountDto.InvoicePaymentCurrency = Convert.ToString(getAccountResponseType.AccountSummary.InvoicePayment.currencyID);
            }

            accountDto.InvoiceCreditValue = 0M;
            if (getAccountResponseType.AccountSummary.InvoiceCredit != null)
            {
                accountDto.InvoiceCreditValue = (decimal)getAccountResponseType.AccountSummary.InvoiceCredit.Value;
                accountDto.InvoiceCreditCurrency = Convert.ToString(getAccountResponseType.AccountSummary.InvoiceCredit.currencyID);
            }

            accountDto.InvoiceNewFeeValue = 0M;
            if (getAccountResponseType.AccountSummary.InvoiceNewFee != null)
            {
                accountDto.InvoiceNewFeeValue = (decimal)getAccountResponseType.AccountSummary.InvoiceNewFee.Value;
                accountDto.InvoiceNewFeeCurrency = Convert.ToString(getAccountResponseType.AccountSummary.InvoiceNewFee.currencyID);
            }

            accountDto.AdditionalAccount.BalanceValue = 0M;
            if (getAccountResponseType.AccountSummary.AdditionalAccount.Any())
            {
                accountDto.AdditionalAccount = new();

                if (getAccountResponseType.AccountSummary.AdditionalAccount.First().Balance != null)
                {
                    accountDto.AdditionalAccount.BalanceValue = (decimal)getAccountResponseType.AccountSummary.AdditionalAccount.First().Balance.Value;
                    accountDto.AdditionalAccount.BalanceCurrency = Convert.ToString(getAccountResponseType.AccountSummary.AdditionalAccount.First().Balance.currencyID);
                }

                accountDto.AdditionalAccount.CurrencyCode = Convert.ToString(getAccountResponseType.AccountSummary.AdditionalAccount.First().Currency);
                accountDto.AdditionalAccount.AccountCode = getAccountResponseType.AccountSummary.AdditionalAccount.First().AccountCode;
            }

            getAccountResponseType.AccountEntries.AccountEntry.ForEach(accountEntry =>
            {
                accountDto.AccountDetails.Add(new AccountDetailDto
                {
                    RefNumber = accountEntry.RefNumber,
                    ItemID = accountEntry.ItemID,
                    Date = accountEntry.Date.GetValueOrDefault(),
                    AccountType = Convert.ToString(accountEntry.AccountDetailsEntryType.GetValueOrDefault()),
                    Title = accountEntry.Title,
                    Description = accountEntry.Description,
                    BalanceValue = (decimal)accountEntry.Balance.Value,
                    BalanceCurrency = accountEntry.Balance.currencyID.ToString(),
                    GrossDetailAmountValue = (decimal)accountEntry.GrossDetailAmount.Value,
                    GrossDetailAmountCurrency = Convert.ToString(accountEntry.GrossDetailAmount.currencyID),
                    ConversionRateValue = (decimal)accountEntry.ConversionRate.Value,
                    ConversionRateCurrency = Convert.ToString(accountEntry.ConversionRate.currencyID),
                    NetDetailAmountValue = (decimal)accountEntry.NetDetailAmount.Value,
                    NetDetailAmountCurrency = Convert.ToString(accountEntry.NetDetailAmount.currencyID),
                    VATPercent = accountEntry.VATPercent.GetValueOrDefault(),
                    OrderLineItemID = accountEntry.OrderLineItemID,
                    TransactionID = accountEntry.TransactionID,
                    ReceivedTopRatedDiscount = accountEntry.ReceivedTopRatedDiscount.GetValueOrDefault()
                });
            });

            return accountDto;
        }
        #endregion
    }
}
