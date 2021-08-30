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

        /// <summary>
        /// 按照时间删除
        /// </summary>
        /// <param name="createDate">创建日期</param>
        public DeleteSystemLogCommand(DateTime createDate)
        {
            CreateDate = createDate;
        }

        /// <summary>
        /// 按照日志类型删除
        /// </summary>
        /// <param name="logType">日志类型</param>
        public DeleteSystemLogCommand(LogType logType)
        {
            LogType = logType;
        }

        public DeleteSystemLogCommand(DateTime createDate,
            LogType logType)
        {
            CreateDate = createDate;
            LogType = logType;
        }
    }
}
