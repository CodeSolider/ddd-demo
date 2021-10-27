using eBay.Service.Core.Sdk;
using EbayPlatform.Infrastructure.Core;
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

        public DownloaderProvider(string shopName, string paramValue)
        {
            ShopName = shopName;
            ParamValue = paramValue;
        }

        /// <summary>
        /// 开始执行处理
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            var apiCall = DownloadingPre?.Invoke(ParamValue);

            var apiResult = await (Downloading?.Invoke(apiCall, ShopName)).ConfigureAwait(false);
            DownloadingEnd?.Invoke(apiResult, ShopName);
        }
    }
}
