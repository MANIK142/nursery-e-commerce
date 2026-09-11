
using BuildingBlocks.Common.CQRS;

namespace Nursery.Orders.Application.Features.DecreaseCartItemQuantity;

public record DecreaseCartItemQuantityCommand(Guid CustomerId, Guid PlantVariantId) : ICommand<DecreaseCartItemQuantityResult>;

public record DecreaseCartItemQuantityResult(bool IsSuccess);