

using Nursery.ShareKernel;

namespace Nursery.Shippings.Domain.Models;

public class TrackingInfo
{
    public string Carrier { get; }
    public string TrackingNumber { get; }
    public string? TrackingUrl { get; }

    private TrackingInfo(string carrier, string trackingNumber, string? trackingUrl)
    {
        Carrier = carrier;
        TrackingNumber = trackingNumber;
        TrackingUrl = trackingUrl;
    }

    public static TrackingInfo Of(string carrier, string trackingNumber, string? trackingUrl = null)
    {
        Guard.AgainstNullOrWhiteSpace(carrier, nameof(carrier));
        Guard.AgainstNullOrWhiteSpace(trackingNumber, nameof(trackingNumber));

        return new TrackingInfo(carrier, trackingNumber, trackingUrl);
    }

}
