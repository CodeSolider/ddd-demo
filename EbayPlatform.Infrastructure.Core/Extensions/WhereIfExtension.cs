using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions; 

namespace EbayPlatform.Infrastructure.Core.Extensions
{
    /// <summary>
    /// Where 扩展
    /// </summary>
    public static class WhereIfExtension
    {
        /// <summary>
        /// where If 扩展函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (condition)
            {
                source = source.Where(predicate);
            }
            return source;
        }

        /// <summary>
        /// where If 扩展函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            if (condition)
            {
                source = source.Where(predicate);
            }
            return source;
        }
    }
}
