
using BuildingBlocks.Common.CQRS;

namespace Nursery.Orders.Application.Features.IncreaseCartItemQuantity;

public record IncreaseCartItemQuantityCommand(Guid CustomerId, Guid PlantVariantId) : ICommand<IncreaseCartItemQuantityResult>;

public record IncreaseCartItemQuantityResult(bool IsSuccess);