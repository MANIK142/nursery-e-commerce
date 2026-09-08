

using BuildingBlocks.Common.CQRS;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Application.Exceptions;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Application.Catagories.Command.UpdateCategory;

public class UpdateCategoryHandler(ICatalogRepository catalogRepository) : ICommandHandler<UpdateCategoryCommand, UpdateCategoryResult>
{
    private readonly ICatalogRepository catalogRepository = catalogRepository;

    public async Task<UpdateCategoryResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await catalogRepository.GetCategoryById(request.Id, cancellationToken);

        if (category == null)
        {
            throw new ItemNotFoundException("Category not found");
        }
        category.Rename(request.Name,request.UpdatedBy);
        category.UpdateDescription(request.Description, request.UpdatedBy);
        var response = await catalogRepository.UpdateCategoryAsync(category, cancellationToken);
        return new UpdateCategoryResult(response);
    }
}
