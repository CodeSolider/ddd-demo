using MediatR;
using System;

namespace EbayPlatform.Domain.Commands.StystemLog
{
    /// <summary>
    /// 删除系统日志
    /// </summary>
    public class DeleteSystemLogCommand : IRequest<bool>
    {
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; }

        /// <summary>
        /// 按照日志删除
        /// </summary>
        /// <param name="createDate"></param>
        public DeleteSystemLogCommand(DateTime createDate)
        {
            CreateDate = createDate;
        }
    }
}
