namespace EbayPlatform.Application.Dto
{
    public class SyncTaskJobConfigDto
    {
        /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 作业注解
        /// </summary>
        public string JobDesc { get; set; }

        /// <summary>
        /// 任务类全名
        /// </summary>
        public string JobClassFullName { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// CronDesc注解
        /// </summary>
        public string CronDesc { get; set; }
    }
}
