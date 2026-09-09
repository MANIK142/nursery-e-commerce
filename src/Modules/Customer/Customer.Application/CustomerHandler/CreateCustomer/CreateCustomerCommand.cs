
using BuildingBlocks.Common.CQRS;
using FluentValidation;

namespace Customer.Application.CustomerHandler.CreateCustomer;
public record CreateCustomerCommand(string FirstName, string LastName, string ExternalUserId, string EmailAddress, string PhoneNumber) : ICommand<CreateCustomerResult>;

public record CreateCustomerResult(bool IsSuccess);

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.FirstName)
             .NotEmpty().WithMessage("First name is required.")
             .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

        RuleFor(x => x.ExternalUserId)
            .NotEmpty().WithMessage("ExternalUserId is required.")
            .MaximumLength(200); // typical max length for IdP subject identifiers / Identity keys

        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(100).WithMessage("Email address must not exceed 256 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{7,14}$")
            .WithMessage("Phone number must be a valid number (E.164 format recommended, e.g. +919876543210).");
    }
}
