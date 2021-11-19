using System;

namespace EbayPlatform.Application.Dtos
{
    /// <summary>
    /// 数据下载分页
    /// </summary>
    public class ParamValueToEntityDto
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageSize { get; set; } = 100;

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 从什么时间开始下载
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// 到什么时间结束下载
        /// </summary>
        public DateTime ToDate { get; set; }
    }
    internal class ParamValueToEntityDto<T> : ParamValueToEntityDto
    {
        public T Data { get; set; }

        /// <summary>
        /// 是否还有下一页
        /// </summary>
        public bool HasNextPage { get; set; } = false;
    }
}
