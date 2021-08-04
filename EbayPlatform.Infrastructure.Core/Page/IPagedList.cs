using System.Collections.Generic;

namespace EbayPlatform.Infrastructure.Core.Page
{
    /// <summary>
    /// 分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPagedList<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// 页码
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// 总记录数
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// 总页数
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// 数据结合
        /// </summary>
        IList<T> Items { get; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        bool HasNextPage { get; }
    }
}
