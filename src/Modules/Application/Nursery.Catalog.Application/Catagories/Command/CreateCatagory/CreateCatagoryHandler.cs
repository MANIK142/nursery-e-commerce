

using BuildingBlocks.Common.CQRS;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Application.Exceptions;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Application.Catagories.Command.CreateCatagory;

public class UpdateCatagoryHandler(ICatalogRepository catalogRepository) : ICommandHandler<CreateCategoryCommand, CreateCategoryResut>
{
    private readonly ICatalogRepository catalogRepository = catalogRepository;

    public async Task<CreateCategoryResut> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var isExists = await catalogRepository.IsCategoryExistsAsync(request.Name, cancellationToken);
        if(isExists) {
            throw new ItemAlreadyExistsException("Already Category available!!!");
        }
        var category = Category.Create(request.Name, request.Description, request.CreatedBy);
        var response = await catalogRepository.CreateCategoryAsync(category, cancellationToken);

        return new CreateCategoryResut(response.Id);
    }
}
