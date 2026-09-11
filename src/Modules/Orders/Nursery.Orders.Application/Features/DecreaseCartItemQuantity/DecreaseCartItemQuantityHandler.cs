using BuildingBlocks.Common.CQRS;
using Nursery.Catalog.Application.Exceptions;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Application.Features.RemoveItemFromCart;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Orders.Application.Features.DecreaseCartItemQuantity;

public class DecreaseCartItemQuantityHandler(ICartRepository repository) : ICommandHandler<DecreaseCartItemQuantityCommand, DecreaseCartItemQuantityResult>
{
    public async Task<DecreaseCartItemQuantityResult> Handle(DecreaseCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var cart = await repository.GetActiveCartByCustomerIdAsync(request.CustomerId, cancellationToken)
                   ?? throw new ItemNotFoundException($"Cart  not found for Customer Id {request.CustomerId}");

        var cartItem = cart.Items.Where(i => i.PlantvariantId == request.PlantVariantId).FirstOrDefault();
        if (cartItem != null)
        {
            cart.DecreaseItemQuantity(request.PlantVariantId);

            var result = await repository.SaveChangesAsync(cancellationToken);

            return new DecreaseCartItemQuantityResult(result);
        }
        throw new ItemNotFoundException($"Plant variant  not found for plantvariant Id {request.PlantVariantId}");
    }
}
