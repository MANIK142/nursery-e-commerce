
using Microsoft.EntityFrameworkCore;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Domain.Carts;
using Nursery.Orders.Domain.Enums;
using Nursery.Orders.Infrastructure.Persistance.Context;

namespace Nursery.Orders.Infrastructure.Persistance.Repository;

public class CartReposiotry(OrdersDbContext dbContext) : ICartRepository
{
    
    public async Task AddAsync(Cart cart, CancellationToken ct)
    {
        await dbContext.Carts.AddAsync(cart,ct);
    }

    public async Task<Cart?> GetActiveCartByCustomerIdAsync(Guid customerId, CancellationToken ct)
    {
        return await dbContext.Carts.Include(c =>c.Items).FirstOrDefaultAsync(c  => c.CustomerId == customerId && 
                                                                                    c.CartStatus == CartStatus.Active);
    }

    public async Task<Cart?> GetByIdAsync(Guid cartId, CancellationToken ct)
    {
        return await dbContext.Carts.Include(c=> c.Items).FirstOrDefaultAsync(c => c.Id == cartId,ct);
    }


    public async Task<bool> SaveChangesAsync(CancellationToken ct)
    {
        var affecteRows =  await dbContext.SaveChangesAsync(ct);
        return affecteRows > 0;
    }
}
