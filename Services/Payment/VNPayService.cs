using FoodFlowSystem.DTOs.Requests.Payment;
using FoodFlowSystem.DTOs.Requests.Payment.PaymentConfigs;
using FoodFlowSystem.DTOs.Responses.Payments;
using FoodFlowSystem.Helpers.VNPay;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;

namespace FoodFlowSystem.Services.Payment
{
    public class VNPayService : IVNPayService
    {
        private readonly VNPayConfig _vnPayConfig;

        public VNPayService(IHttpContextAccessor httpContextAccessor, IOptions<VNPayConfig> vnPayConfig)
        {
            _vnPayConfig = vnPayConfig.Value;
        }

        public string CreatePaymentUrl(VNPayRequest request, string ipAddress)
        {
            var pay = new VnPayLibrary();

            pay.AddRequestData("vnp_Version", _vnPayConfig.Version);
            pay.AddRequestData("vnp_Command", "pay");
            pay.AddRequestData("vnp_TmnCode", _vnPayConfig.TmnCode);
            pay.AddRequestData("vnp_Amount", ((long)(request.Amount * 100)).ToString());
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_IpAddr", ipAddress);
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_OrderInfo", request.OrderInfo);
            pay.AddRequestData("vnp_OrderType", "food");
            pay.AddRequestData("vnp_ReturnUrl", _vnPayConfig.ReturnUrl);
            pay.AddRequestData("vnp_TxnRef", request.OrderId.ToString());

            return pay.CreateRequestUrl(_vnPayConfig.PaymentUrl, _vnPayConfig.HashSecret);
        }

        public VNPayResponse ProcessPaymentCallback(IQueryCollection query)
        {
            try
            {
                var pay = new VnPayLibrary();
                var vnpayData = new Dictionary<string, string>();
                foreach (var key in query.Keys)
                {
                    vnpayData.Add(key, query[key]);
                }

                if (!ValidateSignature(vnpayData, _vnPayConfig.HashSecret))
                {
                    return new VNPayResponse
                    {
                        OrderId = int.Parse(vnpayData["vnp_TxnRef"]),
                        ResponseCode = "97",
                        Message = "Chữ ký không hợp lệ",
                    };
                }

                var response = new VNPayResponse();
                response.OrderId = int.Parse(vnpayData["vnp_TxnRef"]);
                response.TransactionId = vnpayData["vnp_TransactionNo"];
                response.ResponseCode = vnpayData["vnp_ResponseCode"];
                response.Amount = Convert.ToDecimal(vnpayData["vnp_Amount"]) / 100;
                response.Message = GetResponseMessage(vnpayData["vnp_ResponseCode"]);
                response.PaymentMethod = vnpayData["vnp_CardType"];
                response.PaymentDate = DateTime.ParseExact(vnpayData["vnp_PayDate"], "yyyyMMddHHmmss", null);

                return response;
            }
            catch (Exception ex)
            {
                return new VNPayResponse
                {
                    ResponseCode = "99",
                    Message = "Xử lý callback thất bại: " + ex.Message,
                };
            }
        }

        public bool ValidateSignature(Dictionary<string, string> vnpayData, string secretKey)
        {
            if (vnpayData.Count == 0)
                return false;

            if (!vnpayData.TryGetValue("vnp_SecureHash", out string secureHash))
                return false;

            var dataWithoutHash = new Dictionary<string, string>(vnpayData);
            dataWithoutHash.Remove("vnp_SecureHash");
            dataWithoutHash.Remove("vnp_SecureHashType");

            var sortedData = new SortedList<string, string>(new VnPayCompare());
            foreach (var kv in dataWithoutHash)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    sortedData.Add(kv.Key, kv.Value);
                }
            }

            var signData = new StringBuilder();
            foreach (var kv in sortedData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    signData.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }

            // Remove last '&'
            var rawData = signData.ToString();
            if (rawData.Length > 0)
            {
                rawData = rawData.Remove(rawData.Length - 1, 1);
            }

            var pay = new VnPayLibrary();
            var calculatedHash = pay.HmacSHA512(secretKey, rawData);

            return calculatedHash.Equals(secureHash, StringComparison.OrdinalIgnoreCase);
        }

        private string GetResponseMessage(string responseCode)
        {
            var messages = new Dictionary<string, string>
            {
                { "00", "Giao dịch thành công" },
                { "01", "Giao dịch đã tồn tại" },
                { "02", "Merchant không hợp lệ" },
                { "03", "Ngân hàng không hợp lệ" },
                { "04", "Giao dịch không hợp lệ" },
                { "05", "Giao dịch không được phép" },
                { "06", "Ngân hàng từ chối giao dịch" },
                { "07", "Giao dịch đã bị hủy" },
                { "08", "Giao dịch đang chờ xử lý" },
                { "09", "Giao dịch đã bị hủy bởi Merchant" },
                { "10", "Giao dịch đã bị hủy bởi Ngân hàng" },
                { "11", "Giao dịch đã bị hủy bởi hệ thống" }
            };

            return messages.GetValueOrDefault(responseCode, "Lỗi không xác định");
        }
    }
}
