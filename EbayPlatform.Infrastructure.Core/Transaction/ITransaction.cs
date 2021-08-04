using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Core.Transaction
{
    /// <summary>
    /// 事务
    /// </summary>
    public interface ITransaction
    {
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
