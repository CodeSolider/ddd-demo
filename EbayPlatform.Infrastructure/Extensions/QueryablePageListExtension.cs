using EbayPlatform.Infrastructure.Page;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Extensions
{
    /// <summary>
    /// 分页扩展类
    /// </summary>
    public static class QueryablePageListExtension
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="indexFrom"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source,
            int pageIndex = 1, int pageSize = 10
            , CancellationToken cancellationToken = default)
        {
            var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await source.Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

            var pagedList = new PagedList<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Items = items,
                TotalCount = count
            };
            return pagedList;
        }
    }
}
