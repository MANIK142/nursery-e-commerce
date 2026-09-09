using FluentValidation;
using Nursery.Identity.Models.DTO;

namespace Nursery.Identity.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name Can't be empty");
    }
}
