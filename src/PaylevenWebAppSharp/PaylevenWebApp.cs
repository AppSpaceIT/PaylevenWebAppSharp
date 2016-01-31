using System;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PaylevenWebAppSharp.Enums;
using PaylevenWebAppSharp.Extensions;

namespace PaylevenWebAppSharp
{
    public class PaylevenWebApp
    {
        private readonly string _token;

        public PaylevenWebApp(string token)
        {
            _token = token;
        }

        private static UriBuilder BuildBaseUrl(string operation, BaseRequest baseRequest)
        {
            return new UriBuilder(string.Format("{0}/{1}/{2}/", Consts.BaseUrl, operation, Consts.Version))
                .AddQuery("domain", baseRequest.CallbackUri.Host)
                .AddQuery("scheme", baseRequest.CallbackUri.Scheme)
                .AddQuery("callback", baseRequest.CallbackUri.PathAndQuery)
                .AddQuery("appName", baseRequest.DisplayName);
        }

        private static void ValidateBaseRequestAndThrow(BaseRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("request");
            }

            if (request.CallbackUri == null)
            {
                throw new ArgumentNullException("CallbackUri", "cannot be null.");
            }

            if (string.IsNullOrEmpty(request.DisplayName))
            {
                throw new ArgumentNullException("DisplayName", "cannot be empty.");
            }
        }

        private static void ValidateBaseOrderRequestAndThrow(BaseOrderRequest request)
        {
            ValidateBaseRequestAndThrow(request);

            if (string.IsNullOrEmpty(request.OrderId))
            {
                throw new ArgumentNullException("OrderId", "cannot be empty.");
            }
        }

        public string GetPaymentUrl(PaymentRequest request)
        {
            ValidateBaseOrderRequestAndThrow(request);

            if (request.PriceInCents <= 0)
            {
                throw new ArgumentOutOfRangeException("PriceInCents", request.PriceInCents,
                    "cannot be less than or equal to 0.");
            }

            var builder = BuildBaseUrl("payment", request)
                .AddQuery("price", request.PriceInCents.ToString())
                .AddQuery("currency", Enum.GetName(typeof (Currencies), request.Currency))
                .AddQuery("description", request.Description)
                .AddQuery("orderId", request.OrderId);

            return builder.ComputeAndAppendSha256ToUri(_token).ToString();
        }

        public string GetRefundUrl(RefundRequest request)
        {
            ValidateBaseOrderRequestAndThrow(request);

            var builder = BuildBaseUrl("refund", request)
                .AddQuery("orderId", request.OrderId);

            return builder.ComputeAndAppendSha256ToUri(_token).ToString();
        }

        public string GetDetailsUrl(DetailsRequest request)
        {
            ValidateBaseOrderRequestAndThrow(request);

            var builder = BuildBaseUrl("trxdetails", request)
                .AddQuery("orderId", request.OrderId);

            return builder.ComputeAndAppendSha256ToUri(_token).ToString();
        }

        public string GetPaymentsHistoryUrl(PaymentsHistoryRequest request)
        {
            ValidateBaseRequestAndThrow(request);

            var builder = BuildBaseUrl("history", request);

            return builder.ComputeAndAppendSha256ToUri(_token).ToString();
        }

        public PaylevenResponse ValidateResponse(HttpRequestBase httpRequest)
        {
            var result = httpRequest.QueryString["result"];

            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentNullException("result", "cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(httpRequest["timestamp"]))
            {
                throw new ArgumentNullException("timestamp", "cannot be empty.");
            }

            var builder = new UriBuilder("localhost");
            foreach (var value in Consts.HashableResponseKeys)
            {
                builder.AddQuery(value, httpRequest[value].GetValueOrEmpty());
            }

            var token = result == Results.LoginCanceled.GetDescription()
                ? Consts.DefaultToken
                : _token;

            var hmac = builder.Query
                .Remove(0, 1)
                .ToSha256(token);

            if (hmac != httpRequest["hmac"])
            {
                throw new Exception("Invalid hmac");
            }

            var response = new PaylevenResponse
            {
                Result = EnumHelper<Results>.GetValueFromDescription(result),
                ErrorCode = string.IsNullOrWhiteSpace(httpRequest["errorCode"])
                    ? ErrorCodes.NoError
                    : EnumHelper<ErrorCodes>.ParseEnum(httpRequest["errorCode"]),
                Currency = string.IsNullOrWhiteSpace(httpRequest["currency"])
                    ? (Currencies?)null
                    : EnumHelper<Currencies>.ParseEnum(httpRequest["currency"]),
            };

            var type = typeof (PaylevenResponse);

            foreach (var property in type.GetProperties())
            {
                var attribute = property
                    .GetCustomAttributes(typeof (DescriptionAttribute), true)
                    .FirstOrDefault() as DescriptionAttribute;

                if (attribute != null && !string.IsNullOrWhiteSpace(httpRequest[attribute.Description]))
                {
                    property.SetValue(response,
                        Convert.ChangeType(httpRequest[attribute.Description], property.PropertyType));
                }
            }

            return response;
        }
    }
}