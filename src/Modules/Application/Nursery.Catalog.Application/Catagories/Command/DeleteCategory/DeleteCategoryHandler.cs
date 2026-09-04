

using BuildingBlocks.Common.CQRS;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Application.Exceptions;

namespace Nursery.Catalog.Application.Catagories.Command.DeleteCategory;

public class DeleteCategoryHandler(ICatalogRepository catalogRepository) : ICommandHandler<DeleteCategoryCommand, DeleteCategoryResult>
{
    private readonly ICatalogRepository catalogRepository = catalogRepository;

    async Task<DeleteCategoryResult> IRequestHandler<DeleteCategoryCommand, DeleteCategoryResult>.Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await catalogRepository.GetCategoryById(request.Id, cancellationToken);
        if (category is null)
        {
            throw new ItemNotFoundException($"Category with Id {request.Id} not found.");
        }
        var result = await catalogRepository.DeleteCategoryById(request.Id, cancellationToken);
        return new DeleteCategoryResult(result);
    }
}
