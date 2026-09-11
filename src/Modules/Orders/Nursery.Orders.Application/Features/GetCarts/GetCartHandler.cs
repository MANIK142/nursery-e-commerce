
using BuildingBlocks.Common.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Application.Dto;

namespace Nursery.Orders.Application.Features.GetCarts;

public class GetCartHandler(IOrdersDbContext dbContext) : IQueryHandler<GetCartQuery, GetCartResult>
{
    public async Task<GetCartResult?> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
                       .AsNoTracking()
                       .Include(c => c.Items)
                       .Where(c => c.CustomerId == request.CustomerId)
                       .Select(c => new CartDto
                       (
                            c.Id,
                            c.CustomerId,
                            c.CartStatus,
                            c.Items.Sum(i => i.Quantity * i.UnitPrice),
                            c.Items.Select(i => new CartItemDto(i.PlantvariantId,i.Quantity,i.UnitPrice)).ToList()
                       ))
                       .FirstOrDefaultAsync(cancellationToken);

        return new GetCartResult(cart);
    }
}
