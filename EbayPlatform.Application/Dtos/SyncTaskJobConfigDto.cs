using System.Collections.Generic;

namespace EbayPlatform.Application.Dtos
{
    public class SyncTaskJobConfigDto
    {
        public SyncTaskJobConfigDto()
        {
            SyncTaskJobParams = new List<ShopTaskDto>();
        }

        /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 作业注解
        /// </summary>
        public string JobDesc { get; set; }

        /// <summary>
        /// Job程序集
        /// </summary>
        public string JobAssemblyName { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// CronDesc注解
        /// </summary>
        public string CronDesc { get; set; }

        /// <summary>
        /// 添加同步任务参数数据
        /// </summary>
        public List<ShopTaskDto> SyncTaskJobParams { get; set; }
    }
}
