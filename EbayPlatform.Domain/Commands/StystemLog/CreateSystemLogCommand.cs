using EbayPlatform.Domain.AggregateModel.Enums;
using MediatR;

namespace EbayPlatform.Domain.Commands.StystemLog
{
    /// <summary>
    /// 创建系统日志
    /// </summary>
    public class CreateSystemLogCommand : IRequest<int>
    {
        /// <summary>
        /// 外键ID->FK
        /// </summary>
        public string ObjectId { get; }

        /// <summary>
        /// 日志类型 
        /// </summary>
        public LogType LogType { get; }

        /// <summary>
        /// Log 内容
        /// </summary>
        public string Content { get; }


        public CreateSystemLogCommand(string objectId, LogType logType, string content)
        {
            this.ObjectId = objectId;
            this.LogType = logType;
            this.Content = content;
        }
    }
}
