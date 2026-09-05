

namespace Nursery.Catalog.Application.Plants.Commands.EndSaleEarly;

public record EndSaleEarlyCommand(Guid PlantId, Guid VariantId,string Modifiedby) : ICommand<EndSaleEarlyResult>;
public record EndSaleEarlyResult(bool IsSuccess);