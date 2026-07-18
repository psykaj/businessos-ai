namespace backend.Modules.Billing.Interfaces;

public interface IRazorpayService
{
    Task<(string OrderId, decimal Amount, string Key)> CreateOrderAsync(decimal amount, string currency, string receipt);
    bool VerifySignature(string paymentId, string orderId, string signature);
}
