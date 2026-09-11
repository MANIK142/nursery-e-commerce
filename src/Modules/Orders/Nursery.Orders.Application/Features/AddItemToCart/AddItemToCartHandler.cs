
using BuildingBlocks.Common.CQRS;
using Microsoft.EntityFrameworkCore;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Application.Exceptions;
using Nursery.Catalog.Domain.Models;
using Nursery.Catalog.Infrastructure.Persistence.Context;
using Nursery.Catalog.Infrastructure.Repository;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Domain.Carts;

namespace Nursery.Orders.Application.Features.AddItemToCart;

public class AddItemToCartHandler(ICartRepository repository, CatalogDbContext catalogDb) : ICommandHandler<AddItemToCartCommand, AddItemToCartResult>
{
    public async Task<AddItemToCartResult> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var plantVariant = await catalogDb.PlantVariants.FirstOrDefaultAsync(p => p.Id == request.PlantvariantId,cancellationToken)
                                 ?? throw new ItemNotFoundException($"Plant variant not found for plant variant Id {request.PlantvariantId}");
      
        var cart = await repository.GetActiveCartByCustomerIdAsync(request.CustomerId,cancellationToken);
        if(cart == null)
        {
            cart = Cart.Create(request.CustomerId);
            await repository.AddAsync(cart, cancellationToken);
        }
        var price = plantVariant.GetPrice(CustomerTier.Retail, DateTime.UtcNow);
        cart.AddcartItem(plantVariant.Id, price.Amount);

       var result =  await repository.SaveChangesAsync(cancellationToken);

        return new AddItemToCartResult(result);

    }
}
