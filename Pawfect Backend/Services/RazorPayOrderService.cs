using System.Security.Cryptography;
using System.Text;
using Pawfect_Backend.Context;
using Pawfect_Backend.Dto;
using Pawfect_Backend.Models;
using Razorpay.Api;

public interface IRazorpayOrderService
{
    Task<Responses<string>> CreateRazorpayOrder(int price);
    Task<Responses<bool>> RazorPayment(PaymentDto payment);
}

public class RazorpayOrderService : IRazorpayOrderService
{
    private readonly string _razorpayKey;
    private readonly string _razorpaySecret;
    private readonly ApplicationDbContext _context;

    public RazorpayOrderService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _razorpayKey = configuration["Razorpay:Key"];
        _razorpaySecret = configuration["Razorpay:Secret"];
    }

    // ✅ Create Razorpay Order
    public async Task<Responses<string>> CreateRazorpayOrder(int price)
    {
        try
        {
            if (price <= 0)
            {
                return new Responses<string> { StatusCode = 400, Message = "Enter a valid price" };
            }

            Dictionary<string, object> input = new Dictionary<string, object>
            {
                { "amount", price * 100 },  // Convert to paise
                { "currency", "INR" },
                { "receipt", Guid.NewGuid().ToString() }
            };

            RazorpayClient client = new RazorpayClient(_razorpayKey, _razorpaySecret);
            Order order = client.Order.Create(input);
            string orderId = order["id"].ToString();

            return new Responses<string> { StatusCode = 200, Message = "OrderId created successfully", Data = orderId };
        }
        catch (Exception ex)
        {
            return new Responses<string> { StatusCode = 500, Message = "Error creating Razorpay order: " + ex.Message };
        }
    }

    // ✅ Verify Razorpay Payment
    public async Task<Responses<bool>> RazorPayment(PaymentDto payment)
    {
        if (payment == null ||
            string.IsNullOrEmpty(payment.razorpay_payment_id) ||
            string.IsNullOrEmpty(payment.razorpay_order_id) ||
            string.IsNullOrEmpty(payment.razorpay_signature))
        {
            return new Responses<bool> { StatusCode = 400, Message = "Payment credentials not found" };
        }

        try
        {
            string generatedSignature = GenerateSignature(payment.razorpay_payment_id, payment.razorpay_order_id, _razorpaySecret);

            if (generatedSignature == payment.razorpay_signature   )
            {
                return new Responses<bool> { StatusCode = 200, Message = "Payment successful", Data = true };
            }
            else
            {
                return new Responses<bool> { StatusCode = 400, Message = "Invalid signature verification failed" };
            }
        }
        catch (Exception ex)
        {
            return new Responses<bool> { StatusCode = 500, Message = "Error while verifying Razorpay payment: " + ex.Message };
        }
    }

    // ✅ Generate HMAC SHA256 Signature (Fixes Verification Issue)
    private string GenerateSignature(string paymentId, string orderId, string secret) 
    {
        string stringToSign = orderId + "|" + paymentId;
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
        {
            var hashBytes =  .ComputeHash(Encoding.UTF8.GetBytes(stringToSign));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
