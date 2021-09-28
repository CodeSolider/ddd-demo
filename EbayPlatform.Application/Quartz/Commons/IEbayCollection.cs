using DotNetCore.CAP;
using eBay.Service.Core.Sdk;
using EbayPlatform.Domain.IntegrationEvents;
using EbayPlatform.Infrastructure.Core;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Quartz.Commons
{
    /// <summary>
    /// Ebay 数据采集
    /// </summary>
    public interface IEbayCollection : ICapSubscribe
    {
        /// <summary>
        /// 接收事件
        /// </summary>
        /// <param name="integrationEvent"></param>
        /// <returns></returns>
        Task ReceiveCapMQEventAsync(CollectionIntegrationEvent integrationEvent);

        /// <summary>
        /// 请求前
        /// </summary>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        ApiCall BeforeRequest(string paramValue);

        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="apiCall"></param>
        /// <returns></returns>
        Task<ApiResult> DownloadDataAsync(ApiCall apiCall);

        /// <summary>
        /// 保存结果
        /// </summary>
        /// <param name="integrationEvent"></param>
        /// <param name="apiResult"></param>
        /// <returns></returns>
        Task SaveDataAsync(CollectionIntegrationEvent integrationEvent, ApiResult apiResult);
    }
}
