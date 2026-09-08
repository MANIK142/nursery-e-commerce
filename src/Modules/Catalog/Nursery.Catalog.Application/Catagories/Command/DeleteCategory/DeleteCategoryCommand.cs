
using BuildingBlocks.Common.CQRS;

namespace Nursery.Catalog.Application.Catagories.Command.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : ICommand<DeleteCategoryResult>;

public record DeleteCategoryResult(bool IsDeleted);
