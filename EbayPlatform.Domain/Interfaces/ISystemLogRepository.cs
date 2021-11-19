using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.AggregateModel;
using System; 
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
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<SystemLog>> GetExpireSystemLogListAsync(DateTime createDate, CancellationToken cancellationToken = default);
    }
}
