using EbayPlatform.Application.Services;
using EbayPlatform.Infrastructure.Core;
using EbayPlatform.WebApi.RequestModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EbayPlatform.WebApi.Controllers
{
    /// <summary>
    /// 同步任务配置接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SyncTaskJobController : ControllerBase
    {
        /// <summary>
        /// 同步任务配置服务
        /// </summary>
        private readonly ISyncTaskJobAppService _syncTaskJobAppService;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public SyncTaskJobController(ISyncTaskJobAppService syncTaskJobAppService)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            this._syncTaskJobAppService = syncTaskJobAppService;
        }

        /// <summary>
        /// 启动所有任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> StartAllTaskJobAsync()
        {
            try
            {
                await _syncTaskJobAppService.ExecuteAllTaskAysnc(HttpContext.RequestAborted).ConfigureAwait(false);
                return ApiResult.OK("所有任务启动成功");
            }
            catch (Exception ex)
            {
                return ApiResult.OK($"所有任务启动失败,失败原因:{ex.Message}");
            }
        }

        /// <summary>
        /// 创建同步任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> CreateSyncTaskJobAsync(CreateSyncTaskJobRequestInput input)
        {
            try
            {
                int syncTaskJobConfigId = await _syncTaskJobAppService
                                                   .CreateSyncTaskJobAsync(input.SyncTaskJobConfig, HttpContext.RequestAborted)
                                                   .ConfigureAwait(false);

                return ApiResult.OK("创建同步任务成功", syncTaskJobConfigId);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail($"创建同步任务失败,失败原因:{ex.Message}");
            }
        }
    }
}
