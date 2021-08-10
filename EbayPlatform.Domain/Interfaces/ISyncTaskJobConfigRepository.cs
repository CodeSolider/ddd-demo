using EbayPlatform.Domain.Models;
using System.Collections.Generic;

namespace EbayPlatform.Domain.Interfaces
{
    /// <summary>
    ///  同步任务作业配置仓储接口
    /// </summary>
    public interface ISyncTaskJobConfigRepository : IRepository<SyncTaskJobConfig, int>
    {
        /// <summary>
        /// 获取所有的任务配置作业数据
        /// </summary>
        /// <returns></returns>
        List<SyncTaskJobConfig> GetSyncTaskJobConfigList();

        /// <summary>
        /// 检查任务名称是否重复
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        bool CheckJobName(string jobName);
    }
}
