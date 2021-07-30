using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Domain.Models;
using EbayPlatform.Infrastructure.Extensions;
using EbayPlatform.Infrastructure.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Context
{
    public class StudentDbContext : DbContext, IUnitOfWork
    {
        protected readonly IMediator _mediator;
        public StudentDbContext(DbContextOptions<StudentDbContext> options,
            IMediator mediator
            ) : base(options)
        {
            _mediator = mediator; 
        }

        /// <summary>
        /// 学生类
        /// </summary>
        public DbSet<Student> Students { get; set; }


        /// <summary>
        /// 应用创建
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentMap());
            base.OnModelCreating(modelBuilder);
        }

        #region IUnitOfWork
        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
        {
            _ = await base.SaveChangesAsync(cancellationToken);
            await _mediator.DispatchDomainEventsAsync(this);
            return true;
        }
        #endregion
    }
}
