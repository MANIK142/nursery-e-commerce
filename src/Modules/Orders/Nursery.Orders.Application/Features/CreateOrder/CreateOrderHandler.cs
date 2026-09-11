

using Azure.Core;
using BuildingBlocks.Common.CQRS;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Application.Exceptions;
using Nursery.Catalog.Domain.Models;
using Nursery.Catalog.Infrastructure.Persistence.Context;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Application.Dto;
using Nursery.Orders.Domain.Orders;
using Nursery.Orders.Domain.ValueObjects;

namespace Nursery.Orders.Application.Features.CreateOrder;

public class CreateOrderHandler(IOrderRepository orderRepository, ICatalogLookup catalogLookup) : ICommandHandler<CreateOrderCommand, CreateOrderResponse>
{
    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var billingAddresss = MapAddressFromDto(request.BillingAddress);
        var shippingAddresss = MapAddressFromDto(request.ShippingAddress);

        var newOrder = Order.Create(request.CustomerId, billingAddresss, shippingAddresss);
        foreach(var Item in request.Items){
            var plantVariant = await catalogLookup.GetPlantVariantById(Item.PlantVariantId,cancellationToken)
                                    ?? throw new PlantVariantNotFoundException($"One of the Item is not found with Plant Id {Item.PlantVariantId} , Order cannot be processed");
            var Price = plantVariant.GetPrice(CustomerTier.Retail,DateTime.UtcNow);
            newOrder.AddItem(plantVariant.Id, plantVariant.VariantName, Price.Amount, Item.Quantity);
        }
        await orderRepository.AddOrderAsync(newOrder, cancellationToken);
        var result = await orderRepository.SaveChangesAsync(cancellationToken);

        return new CreateOrderResponse(newOrder.Id);

    }

    public Address MapAddressFromDto(AddressDto Dto)
    {
        return Address.Of(Dto.FirstName, Dto.LastName, Dto.EmailAddress,Dto.AddressLine,Dto.Country,Dto.State,Dto.ZipCode);
    }
}
