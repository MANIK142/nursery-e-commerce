
using MediatR;
namespace BuildingBlocks.Common.IntegrationEvents;

public sealed record UserRegisteredIntegrationEvent(string FirstName,
    string LastName,string ExternalUserId,
    string Email,string PhoneNumber
    ) : INotification;

