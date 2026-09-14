namespace Nursery.Payment.Api.Exceptions
{
    public class PaymentGatewayException : Exception
    {
        public PaymentGatewayException(string message, Exception innerException)
       : base(message, innerException) { }
    }
}
