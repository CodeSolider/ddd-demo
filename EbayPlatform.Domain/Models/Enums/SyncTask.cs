namespace EbayPlatform.Domain.Models.Enums
{
    /// <summary>
    /// 同步任务
    /// </summary>
    public class SyncTask
    {
        /// <summary>
        /// 作业状态
        /// </summary>
        public enum JobStatus
        {
            /// <summary>
            /// 未执行
            /// </summary>
            UnExecute = 0,

            /// <summary>
            /// 正在执行中
            /// </summary>
            Executing = 1,

            /// <summary>
            /// 已完成
            /// </summary>
            Completed = 2
        }
    }
}
