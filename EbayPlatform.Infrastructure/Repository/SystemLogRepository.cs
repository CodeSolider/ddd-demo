using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Domain.Models;
using EbayPlatform.Infrastructure.Context;
using EbayPlatform.Infrastructure.Core.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Repository
{
    public class SystemLogRepository : Repository<SystemLog, int, EbayPlatformDbContext>,
        ISystemLogRepository, IDependency
    {
        public SystemLogRepository(EbayPlatformDbContext dbContext)
            : base(dbContext) { }

        /// <summary>
        /// 获取过期的系统日志
        /// </summary>
        /// <param name="createDate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IPagedList<SystemLog>> GetExpireSystemLogListAsync(DateTime createDate,
            CancellationToken cancellationToken = default)
        {
            return this.DbContext.SystemLogs
                        .Where(o => o.CreateDate.Day == createDate.Day)
                        .ToPagedListAsync(pageSize: 1000, cancellationToken: cancellationToken);
        }
    }
}
