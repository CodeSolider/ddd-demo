using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Domain.Models.Orders;
using EbayPlatform.Infrastructure.Context;
using EbayPlatform.Infrastructure.Core; 

namespace EbayPlatform.Infrastructure.Repository
{
    public class OrderRepository : Repository<Order, long, EbayPlatformDbContext>,
        IOrderRepository, IDependency
    {
        public OrderRepository(EbayPlatformDbContext dbContext)
          : base(dbContext) { }



    }
}
