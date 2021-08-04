using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Infrastructure.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Core
{
    public class EFContext : DbContext, IUnitOfWork
    {
        protected readonly IMediator _mediator;
        public EFContext(DbContextOptions options,
            IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
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
