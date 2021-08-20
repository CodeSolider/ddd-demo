using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Models.Enums;
using System;

namespace EbayPlatform.Domain.Models
{
    /// <summary>
    /// 同步日志
    /// </summary>
    public class SystemLog : Entity<int>, IAggregateRoot
    {
        /// <summary>
        /// 外键ID->FK
        /// </summary>
        public int ObjectId { get; set; }

        /// <summary>
        /// 日志类型 
        /// </summary>
        public LogType LogType { get; set; }

        /// <summary>
        /// Log 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
