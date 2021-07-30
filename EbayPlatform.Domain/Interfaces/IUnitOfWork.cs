using System;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.Interfaces
{
    /// <summary>
    /// 定义工作单元的接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 异步 统一保存
        /// </summary>
        /// <returns></returns>
        Task<bool> CommitAsync(CancellationToken cancellationToken = default);
    }
}
