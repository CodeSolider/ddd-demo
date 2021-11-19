using EbayPlatform.Domain.AggregateModel.Enums; 

namespace EbayPlatform.Application.Dtos
{
    /// <summary>
    /// 系统日志
    /// </summary>
    public class SystemLogDto
    {
        /// <summary>
        /// 外键ID->FK
        /// </summary>
        public string ObjectId { get;  set; }

        /// <summary>
        /// 日志类型 
        /// </summary>
        public LogType LogType { get;  set; }

        /// <summary>
        /// Log 内容
        /// </summary>
        public string Content { get;  set; }
    }
}
