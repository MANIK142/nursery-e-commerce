

using BuildingBlocks.Common.CQRS;

namespace Nursery.Orders.Application.Features.RemoveItemFromCart;

public record RemoveItemFromCartCommand(Guid CustomerId, Guid PlantVariantId) : ICommand<RemoveItemFromCartResult>;
public record RemoveItemFromCartResult(bool IsSuccess);
