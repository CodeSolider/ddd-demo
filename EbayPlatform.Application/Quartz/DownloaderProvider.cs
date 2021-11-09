using eBay.Service.Core.Sdk;
using EbayPlatform.Infrastructure.Core;
using EbayPlatform.Infrastructure.Core.Engines;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Quartz
{
    /// <summary>
    /// 下载处理器
    /// </summary>
    public class DownloaderProvider
    {
        /// <summary>
        /// 下载前
        /// </summary>
        public event Func<string, ApiCall> DownloadingPre;

        /// <summary>
        /// 下载中
        /// </summary>
        public event Func<ApiCall, string, Task<ApiResult>> Downloading;

        /// <summary>
        /// 结束下载
        /// </summary>
        public event Func<ApiResult, string, Task> DownloadingEnd;

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; }

        /// <summary>
        /// 参数
        /// </summary>
        public string ParamValue { get; }

        private readonly ILogger _logger;

        public DownloaderProvider(string shopName, string paramValue)
        {
            ShopName = shopName;
            ParamValue = paramValue;
            _logger = EngineContext.Current.Resolve<ILoggerFactory>().CreateLogger(shopName);
        }

        /// <summary>
        /// 开始执行处理
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            _logger.LogInformation($"参入的参数为：{ParamValue}");
            var apiCall = DownloadingPre?.Invoke(ParamValue);
            var apiResult = await (Downloading?.Invoke(apiCall, ShopName)).ConfigureAwait(false);
            _logger.LogInformation($"下载返回信息：{JsonConvert.SerializeObject(apiResult)}");
            DownloadingEnd?.Invoke(apiResult, ShopName);
        }
    }
}
