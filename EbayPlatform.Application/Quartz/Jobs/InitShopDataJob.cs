using EbayPlatform.Application.Dtos;
using EbayPlatform.Application.Dtos.ScticooErp;
using EbayPlatform.Application.Services;
using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Infrastructure.Core.Engines;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Quartz.Jobs
{
    /// <summary>
    /// 店铺数据初始化
    /// </summary>
    [DisallowConcurrentExecution]
    public class InitShopDataJob : IJob, IDependency
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISyncTaskJobAppService _syncTaskJobAppService;
        public InitShopDataJob()
        {
            _httpClientFactory = EngineContext.Current.Resolve<IHttpClientFactory>();
            _syncTaskJobAppService = EngineContext.Current.Resolve<ISyncTaskJobAppService>();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var httpClient = _httpClientFactory.CreateClient("ScitooErp");
            var httpResponseMessage = await httpClient.PostAsync("api/PlatformShopAccount/getList", new StringContent(JsonConvert.SerializeObject(new
            {
                Platform = "Ebay",
                Status = true,
                IsAuthorized = true,
                limit = int.MaxValue
            }), Encoding.UTF8, "application/json")).ConfigureAwait(false);
            string content = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            var keyValuePairs = JObject.Parse(content);
            var platShopAccountList = keyValuePairs["data"].ToObject<List<PlatformShopAccountDto>>();

            var syncTaskJobConfigList = await _syncTaskJobAppService.GetSyncTaskJobConfigListAsync(ignoreQueryFilter: false).ConfigureAwait(false);
            foreach (var syncTaskJobConfigItem in syncTaskJobConfigList.Where(o => o.SyncErp))
            {
                platShopAccountList.ForEach(platShopAccountItem =>
                {
                    syncTaskJobConfigItem.AddShopTask(platShopAccountItem.PlatformShopAccount.AccountName, JsonConvert.SerializeObject(new ParamValueToEntityDto
                    {
                        Token = platShopAccountItem.PlatformShopAccount.Token,
                        FromDate = DateTime.Now.AddYears(-1),
                        ToDate = DateTime.Now.AddDays(-1).AddDays(1)
                    }));
                });
            }

            if (syncTaskJobConfigList.Any())
            {
                await _syncTaskJobAppService.UpdateShopTaskAsync(syncTaskJobConfigList).ConfigureAwait(false);
            }
        }
    }
}
