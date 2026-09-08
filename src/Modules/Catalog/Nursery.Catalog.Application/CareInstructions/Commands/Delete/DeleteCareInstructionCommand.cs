

using FluentValidation;
using Nursery.Catalog.Application.CareInstructions.Commands.Create;
using Nursery.Catalog.Domain.Enums;

namespace Nursery.Catalog.Application.CareInstructions.Commands.Delete;

public record DeleteCareInstructionCommand(Guid PlantId) : ICommand<DeleteCareInstructionResult>;
public record DeleteCareInstructionResult(bool IsSuccess);
