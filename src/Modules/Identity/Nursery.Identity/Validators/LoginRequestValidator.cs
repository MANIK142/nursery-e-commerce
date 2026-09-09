using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using Nursery.Identity.Models.DTO;

namespace Nursery.Identity.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
