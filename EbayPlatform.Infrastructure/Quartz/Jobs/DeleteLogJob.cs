using EbayPlatform.Domain.Core.Abstractions;
using Quartz;
using System;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Quartz.Jobs
{
    public class DeleteLogJob : IJob, IScopedDependency
    {
        public DeleteLogJob()
        {

        }

        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
