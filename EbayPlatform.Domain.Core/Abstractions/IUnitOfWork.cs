using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.Core.Abstractions
{
    /// <summary>
    /// 定义工作单元的接口
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 异步 统一保存
        /// </summary>
        /// <returns></returns>
        Task<bool> CommitAsync(CancellationToken cancellationToken = default);

        #region 事务
        /// <summary>
        /// 获取当前事物
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction GetCurrentTransaction();

        /// <summary>
        /// 是否有激活的事物
        /// </summary>
        bool HasActiveTransaction { get; }

        /// <summary>
        /// 开始事物
        /// </summary>
        /// <returns></returns>
        Task<IDbContextTransaction> BeginTransactionAsync();

        /// <summary>
        /// 事物提交
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task CommitTransactionAsync(IDbContextTransaction transaction);

        /// <summary>
        /// 事物回滚
        /// </summary>
        void RollbackTransaction();
        #endregion
    }
}
