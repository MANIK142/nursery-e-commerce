using BuildingBlocks.Common.CQRS;
namespace Nursery.Orders.Application.Features.GetCartCount;

public record GetCartCountQuery(Guid CustomerId) : IQuery<GetCartCountResult>;
public record GetCartCountResult(int Count);
