using EbayPlatform.Domain.Models.Enums;
using MediatR;
using System;

namespace EbayPlatform.Domain.Commands.StystemLog
{
    public class DeleteSystemLogCommand : IRequest<bool>
    {
        /// <summary>
        /// PK 外键
        /// </summary>
        public int ObjectId { get; private set; }

        /// <summary>
        /// 日志类型 
        /// </summary>
        public LogType LogType { get; private set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; private set; }



        public DeleteSystemLogCommand() { }
        public DeleteSystemLogCommand(int objectId, LogType logType, DateTime createDate)
        {
            this.ObjectId = objectId;
            LogType = logType;
            CreateDate = createDate;
        }

    }
}
