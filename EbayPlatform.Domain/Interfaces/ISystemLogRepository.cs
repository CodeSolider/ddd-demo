using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Models;
using EbayPlatform.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.Interfaces
{
    /// <summary>
    /// 系统日志接口
    /// </summary>
    public interface ISystemLogRepository : IRepository<SystemLog, int>
    {
        /// <summary>
        /// 获取过期的系统日志
        /// </summary>
        /// <param name="createDate"></param>
        /// <param name="logType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<SystemLog>> GetExpireSystemLogListAsync(DateTime createDate,
               LogType? logType,CancellationToken cancellationToken = default);

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="systemLog"></param>
        void AddSystemLog(SystemLog systemLog);

        /// <summary>
        /// 批量日志添加
        /// </summary>
        /// <param name="systemLogList"></param>
        /// <returns></returns>
        Task AddSystemLogList(List<SystemLog> systemLogList);
    }
}
