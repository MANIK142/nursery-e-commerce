
using BuildingBlocks.Common.CQRS;
using Customer.Application.Dtos;
using Customer.Domain.Models;
using FluentValidation;

namespace Customer.Application.CustomerHandler.UpdateCustomer;

public record UpdateCustomerProfileCommand(string FirstName, string LastName, string Email, string? PhoneNumber,List<AddressDto>? AddressDtos) : ICommand<UpdateCustomerProfileResult>;

public record UpdateCustomerProfileResult(bool IsSuccess);

public class UpdateCustomerProfileVadidator :AbstractValidator<UpdateCustomerProfileCommand>
{
    public UpdateCustomerProfileVadidator()
    {
        RuleFor(x => x.FirstName)
             .NotEmpty().WithMessage("First name is required.")
             .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(100).WithMessage("Email address must not exceed 256 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{7,14}$")
            .WithMessage("Phone number must be a valid number (E.164 format recommended, e.g. +919876543210).");
    }
}