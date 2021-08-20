using EbayPlatform.Application.Dto;
using EbayPlatform.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EbayPlatform.WebApi.Controllers
{
    /// <summary>
    /// 同步任务作业配置
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SyncTaskJobConfigController : ControllerBase
    {
        private readonly ILogger<SyncTaskJobConfigController> _logger;
        private readonly ISyncTaskJobService _syncTaskJobService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="syncTaskJobService"></param>
        public SyncTaskJobConfigController(ILogger<SyncTaskJobConfigController> logger,
            ISyncTaskJobService syncTaskJobService)
        {
            _logger = logger;
            _syncTaskJobService = syncTaskJobService;
        }

        /// <summary>
        /// 执行所有任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult ExecuteAllTask()
        {
            try
            {
                _syncTaskJobService.ExecuteAllTask();
                return ApiResult.OK("执行所有可用任务成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"{nameof(ExecuteAllTask)}发生异常");
                _logger.LogError($"{ex.Message}");
                return ApiResult.Fail("执行所有可用任务发生异常", $"异常信息:{ex.Message}");
            }
        }

        /// <summary>
        /// 创建同步任务
        /// </summary>
        /// <param name="syncTaskJobConfigDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> CreateSyncTaskJob([FromBody] SyncTaskJobConfigDto syncTaskJobConfigDto)
        {
            try
            {
                int syncTaskJobId = await _syncTaskJobService.CreateSyncTaskJobAsync(syncTaskJobConfigDto, HttpContext.RequestAborted).ConfigureAwait(false);
                return ApiResult.OK("创建任务配置信息成功", syncTaskJobId);
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"{nameof(CreateSyncTaskJob)}发生异常");
                _logger.LogError($"{ex.Message}");
                return ApiResult.Fail("创建任务配置信息发生异常", $"异常信息:{ex.Message}");
            }
        }

        /// <summary>
        /// 删除同步任务数据
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        [HttpDelete("{jobName}")]
        public async Task<ApiResult> DeleteSyncTaskJob([FromQuery] string jobName)
        {
            try
            {
                return ApiResult.OK("删除同步任务数据成功", await _syncTaskJobService.DeleteSyncTaskJobAsync(jobName, HttpContext.RequestAborted).ConfigureAwait(false));
            }
            catch (System.Exception ex)
            {
                throw;
            }

        }


    }
}
