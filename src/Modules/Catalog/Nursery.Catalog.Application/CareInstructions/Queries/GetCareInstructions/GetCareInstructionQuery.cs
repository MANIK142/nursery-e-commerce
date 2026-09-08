
namespace Nursery.Catalog.Application.CareInstructions.Queries.GetCareInstructions;
public record GetCareInstructionQuery(int? PageNumber,int? PageSize, Guid? Id,string? FilterBy,string? FilterValue) : IQuery<GetCareInstructionResponse>;
public record GetCareInstructionResponse(
    IEnumerable<CareInstructionDto> CareInstruction
);


