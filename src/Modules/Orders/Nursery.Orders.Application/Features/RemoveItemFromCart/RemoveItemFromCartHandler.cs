using BuildingBlocks.Common.CQRS;
using Nursery.Catalog.Application.Exceptions;
using Nursery.Catalog.Infrastructure.Persistence.Context;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Domain.Carts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Orders.Application.Features.RemoveItemFromCart;

public class RemoveItemFromCartHandler(ICartRepository repository) : ICommandHandler<RemoveItemFromCartCommand, RemoveItemFromCartResult>
{
    public async Task<RemoveItemFromCartResult> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await repository.GetActiveCartByCustomerIdAsync(request.CustomerId, cancellationToken)
                    ?? throw  new ItemNotFoundException($"Cart  not found for Customer Id {request.CustomerId}");

        var cartItem = cart.Items.Where(i => i.PlantvariantId == request.PlantVariantId).FirstOrDefault();
        if (cartItem !=  null)
        {
            cart.RemoveItemFromCart(request.PlantVariantId);

            var result = await repository.SaveChangesAsync(cancellationToken);

            return new RemoveItemFromCartResult(result);
        }
        throw new ItemNotFoundException($"Plant variant  not found for plantvariant Id {request.PlantVariantId}");

    }
}
