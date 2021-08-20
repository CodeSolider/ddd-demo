using EbayPlatform.Domain.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EbayPlatform.Infrastructure.Core.Page
{
    public class PagedList<T> : IPagedList<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; internal set; }

        /// <summary>
        /// 总页码
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>
        /// 数据集合
        /// </summary>
        public IList<T> Items { get; set; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="indexFrom"></param>
        internal PagedList(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            if (source is IQueryable<T> querable)
            {
                PageIndex = pageIndex;
                PageSize = pageSize;
                TotalCount = querable.Count();
                Items = querable.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
            else
            {
                PageIndex = pageIndex;
                PageSize = pageSize;
                TotalCount = source.Count();
                Items = source.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        internal PagedList() => Items = Array.Empty<T>();
    }
}
