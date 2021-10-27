using EbayPlatform.Application.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Services
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ISystemLogAppService
    {
        /// <summary>
        /// 根据创建删除过期的日志
        /// </summary>
        /// <param name="createDate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteSystemLogByDateAsync(DateTime createDate, CancellationToken cancellationToken = default);

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="systemLogDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> AddSystemLogAsync(SystemLogDto systemLogDto, CancellationToken cancellationToken = default);
    }
}
