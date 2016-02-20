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
            return new UriBuilder($"{Consts.BaseUrl}/{operation}/{Consts.Version}/")
                .AddQuery("domain", baseRequest.CallbackUri.Host)
                .AddQuery("scheme", baseRequest.CallbackUri.Scheme)
                .AddQuery("callback", baseRequest.CallbackUri.PathAndQuery.TrimStart('/'))
                .AddQuery("appName", baseRequest.DisplayName);
        }

        private static void ValidateBaseRequestAndThrow(BaseRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException(nameof(request));
            }

            if (request.CallbackUri == null)
            {
                throw new ArgumentNullException(nameof(request.CallbackUri), $"{nameof(request.CallbackUri)} cannot be null.");
            }

            if (string.IsNullOrEmpty(request.DisplayName))
            {
                throw new ArgumentNullException(nameof(request.DisplayName), $"{nameof(request.DisplayName)} cannot be empty.");
            }
        }

        private static void ValidateBaseOrderRequestAndThrow(BaseOrderRequest request)
        {
            ValidateBaseRequestAndThrow(request);

            if (string.IsNullOrEmpty(request.OrderId))
            {
                throw new ArgumentNullException(nameof(request.OrderId), $"{nameof(request.OrderId)} cannot be empty.");
            }
        }

        public string GetPaymentUrl(PaymentRequest request)
        {
            ValidateBaseOrderRequestAndThrow(request);

            if (request.PriceInCents <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(request.PriceInCents), request.PriceInCents,
                    $"{nameof(request.PriceInCents)} cannot be less than or equal to 0.");
            }

            var builder = BuildBaseUrl("payment", request)
                .AddQuery("price", request.PriceInCents.ToString())
                .AddQuery("currency", Enum.GetName(typeof(Currencies), request.Currency))
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
            var timestamp = httpRequest.QueryString["timestamp"];

            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentNullException(nameof(result), $"{nameof(result)} cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(timestamp))
            {
                throw new ArgumentNullException(nameof(timestamp), $"{nameof(timestamp)} cannot be empty.");
            }

            var builder = new UriBuilder("localhost");
            foreach (var value in Consts.HashableResponseKeys)
            {
                builder.AddQuery(value, httpRequest.QueryString[value].GetValueOrEmpty());
            }

            var token = result == Results.LoginCanceled.GetDescription()
                ? Consts.DefaultToken
                : _token;

            var hmac = builder.Query
                .TrimStart('?')
                .ToSha256(token);

            if (hmac != httpRequest.QueryString["hmac"])
            {
                throw new Exception("Invalid hmac");
            }

            var response = new PaylevenResponse
            {
                Result = EnumHelper<Results>.GetValueFromDescription(result),
                ErrorCode = string.IsNullOrWhiteSpace(httpRequest.QueryString["errorCode"])
                    ? ErrorCodes.NoError
                    : EnumHelper<ErrorCodes>.ParseEnum(httpRequest.QueryString["errorCode"]),
                Currency = string.IsNullOrWhiteSpace(httpRequest.QueryString["currency"])
                    ? (Currencies?)null
                    : EnumHelper<Currencies>.ParseEnum(httpRequest.QueryString["currency"]),
            };

            var type = typeof (PaylevenResponse);

            foreach (var property in type.GetProperties())
            {
                var attribute = property
                    .GetCustomAttributes(typeof (DescriptionAttribute), true)
                    .FirstOrDefault() as DescriptionAttribute;

                if (attribute != null && !string.IsNullOrWhiteSpace(httpRequest.QueryString[attribute.Description]))
                {
                    property.SetValue(response,
                        Convert.ChangeType(httpRequest.QueryString[attribute.Description], property.PropertyType));
                }
            }

            return response;
        }
    }
}