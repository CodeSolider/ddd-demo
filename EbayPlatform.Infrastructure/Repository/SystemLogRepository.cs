using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Domain.Models;
using EbayPlatform.Infrastructure.Context;
using EbayPlatform.Infrastructure.Core.Dependency;
using EbayPlatform.Infrastructure.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Repository
{
    public class SystemLogRepository : Repository<SystemLog, int, EbayPlatformDbContext>, ISystemLogRepository, IDependency
    {
        public SystemLogRepository(EbayPlatformDbContext dbContext)
            : base(dbContext) { }


        /// <summary>
        /// 获取过期的系统日志
        /// </summary>
        /// <param name="createDate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<SystemLog>> GetExpireSystemLogListAsync(DateTime createDate,
              CancellationToken cancellationToken = default)
        {
            var query = NoTrackingQueryable.Where(o => o.CreateDate.Day == createDate.Day);
            return await query.ToPagedListAsync(pageSize: 1000, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="systemLog"></param>
        public void AddSystemLog(SystemLog systemLog)
        {
            this.Add(systemLog);
        }

        /// <summary>
        /// 批量日志添加
        /// </summary>
        /// <param name="systemLogList"></param>
        /// <returns></returns>
        public async Task AddSystemLogList(List<SystemLog> systemLogList)
        {
            await this.AddRangeAsync(systemLogList);
        }
    }
}
