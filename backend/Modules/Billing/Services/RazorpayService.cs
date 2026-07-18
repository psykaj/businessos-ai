using backend.Modules.Billing.Interfaces;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;

namespace backend.Modules.Billing.Services;

public class RazorpayService : IRazorpayService
{
    private readonly string _keyId;
    private readonly string _keySecret;

    public RazorpayService(IConfiguration configuration)
    {
        _keyId = configuration["Razorpay:KeyId"] ?? "rzp_test_mockkey";
        _keySecret = configuration["Razorpay:KeySecret"] ?? "rzp_test_mocksecret";
    }

    public async Task<(string OrderId, decimal Amount, string Key)> CreateOrderAsync(decimal amount, string currency, string receipt)
    {
        var client = new RazorpayClient(_keyId, _keySecret);
        
        Dictionary<string, object> options = new Dictionary<string, object>
        {
            { "amount", (int)(amount * 100) }, // amount in smallest currency unit
            { "currency", currency },
            { "receipt", receipt },
            { "payment_capture", 1 }
        };

        // Note: Razorpay C# SDK is mostly synchronous under the hood, wrapping in Task.Run for async flow
        var order = await Task.Run(() => client.Order.Create(options));
        string orderId = order["id"].ToString();
        
        return (orderId, amount, _keyId);
    }

    public bool VerifySignature(string paymentId, string orderId, string signature)
    {
        try
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>
            {
                { "razorpay_payment_id", paymentId },
                { "razorpay_order_id", orderId },
                { "razorpay_signature", signature }
            };

            Utils.verifyPaymentSignature(attributes);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
