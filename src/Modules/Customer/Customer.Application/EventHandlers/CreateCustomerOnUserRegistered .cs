using Azure.Core;
using BuildingBlocks.Common.IntegrationEvents;
using Customer.Application.CustomerHandler.CreateCustomer;
using Customer.Application.Data;
using Customer.Domain.Models;
using MediatR;
using Nursery.Catalog.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Customer.Application.EventHandlers;

public sealed class CreateCustomerOnUserRegistered : INotificationHandler<UserRegisteredIntegrationEvent>
{
    private readonly ICustomerRepository customerRepository;

    public CreateCustomerOnUserRegistered(ICustomerRepository customerRepository)
    {
        this.customerRepository = customerRepository;
    }
    public async Task Handle(UserRegisteredIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var ExistingCustomer = await customerRepository.GetCustomerByEmail(notification.Email, cancellationToken);
        if (ExistingCustomer != null)
            throw new CustomerAlreadyExistsException($"Customer already Exists with this email address {notification.Email}");

        var customer = CustomerEntity.Create(notification.FirstName, notification.LastName, notification.ExternalUserId, notification.Email, notification.PhoneNumber);

        await customerRepository.CreateCustomer(customer, cancellationToken);
    }

}
