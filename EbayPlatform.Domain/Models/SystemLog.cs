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
        public string ObjectId { get; private set; }

        /// <summary>
        /// 日志类型 
        /// </summary>
        public LogType LogType { get; private set; }

        /// <summary>
        /// Log 内容
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; private set; }


        protected SystemLog() { }

        public SystemLog(string objectId, LogType logType, string content)
        {
            this.ObjectId = objectId;
            this.LogType = logType;
            this.Content = content;
        }

    }
}
