using FluentValidation;
using MediatR;
namespace Nursery.Identity.Models.DTO;
public sealed record RefreshTokenRequestDto
{
    public string RefreshToken { get; set; } = default!;
}

public class RefreshTokenRequestCommandValidator : AbstractValidator<RefreshTokenRequestDto>
{
    public RefreshTokenRequestCommandValidator() =>
        RuleFor(x => x.RefreshToken).NotEmpty().MaximumLength(512);
}