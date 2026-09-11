
using BuildingBlocks.Common.CQRS;
using System.Windows.Input;

namespace Nursery.Orders.Application.Features.AddItemToCart;

public record AddItemToCartCommand(Guid CustomerId,Guid PlantvariantId) : ICommand<AddItemToCartResult>;


public record AddItemToCartResult(bool IsSuccess);
