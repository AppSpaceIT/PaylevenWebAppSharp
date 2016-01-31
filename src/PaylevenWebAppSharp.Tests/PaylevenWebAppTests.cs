using System;
using System.Collections.Specialized;
using System.Web;
using NSubstitute;
using NUnit.Framework;
using PaylevenWebAppSharp.Enums;
using PaylevenWebAppSharp.Extensions;
using DeepEqual;
using DeepEqual.Syntax;

namespace PaylevenWebAppSharp.Tests
{
    [TestFixture]
    public class PaylevenWebAppTests
    {
        private const string Token = "fake_token";
        private const string DisplayName = "AppName";
        private const string OrderId = "order_123";
        private readonly Uri _callBackUri = new Uri("https://domain.com/checkout/callback");
        private const string Timestamp = "2016-01-30T11:15:30";

        private readonly PaylevenWebApp _sut = new PaylevenWebApp(Token);
        private readonly HttpRequestBase _httpRequest = Substitute.For<HttpRequestBase>();

        #region GetPaymentUrl
        [Test]
        public void GetPaymentUrl_GeneratesUrl()
        {
            var request = new PaymentRequest
            {
                CallbackUri = _callBackUri,
                DisplayName = DisplayName,
                Currency = Currencies.GBP,
                PriceInCents = 1050,
                Description = @"Some description here",
                OrderId = OrderId
            };

            var actual = _sut.GetPaymentUrl(request);

            Assert.IsNotNull(actual);
        }

        [Test]
        public void GetPaymentUrl_ThrowsWhenNullRequest()
        {
            Assert.Throws<ArgumentException>(() => _sut.GetPaymentUrl(null));
        }

        [Test]
        public void GetPaymentUrl_ThrowsWhenMissingArguments()
        {
            var request = new PaymentRequest();

            Assert.Throws<ArgumentNullException>(() => _sut.GetPaymentUrl(request));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-10)]
        public void GetPaymentUrl_ThrowsWhenPriceInCentsIsEqualOrLessThanZero(int priceInCents)
        {
            var request = new PaymentRequest
            {
                CallbackUri = new Uri("https://domain.extension/callback"),
                DisplayName = "TestShop",
                Currency = Currencies.GBP,
                PriceInCents = priceInCents,
                Description = @"Some description here",
                OrderId = "order_123"
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.GetPaymentUrl(request));
        }
        #endregion

        #region GetRefundUrl
        [Test]
        public void GetRefundUrl_GeneratesUrl()
        {
            var request = new RefundRequest
            {
                CallbackUri = _callBackUri,
                DisplayName = DisplayName,
                OrderId = OrderId
            };

            var actual = _sut.GetRefundUrl(request);

            Assert.IsNotNull(actual);
        }

        [Test]
        public void GetRefundUrl_ThrowsWhenNullRequest()
        {
            Assert.Throws<ArgumentException>(() => _sut.GetRefundUrl(null));
        }

        [Test]
        public void GetRefundUrl_ThrowsWhenMissingArguments()
        {
            var request = new RefundRequest();

            Assert.Throws<ArgumentNullException>(() => _sut.GetRefundUrl(request));
        }
        #endregion

        #region GetDetailsUrl
        [Test]
        public void GetDetailsUrl_GeneratesUrl()
        {
            var request = new DetailsRequest
            {
                CallbackUri = _callBackUri,
                DisplayName = DisplayName,
                OrderId = OrderId
            };

            var actual = _sut.GetDetailsUrl(request);

            Assert.IsNotNull(actual);
        }

        [Test]
        public void GetDetailsUrl_ThrowsWhenNullRequest()
        {
            Assert.Throws<ArgumentException>(() => _sut.GetDetailsUrl(null));
        }

        [Test]
        public void GetDetailsUrl_ThrowsWhenMissingArguments()
        {
            var request = new DetailsRequest();

            Assert.Throws<ArgumentNullException>(() => _sut.GetDetailsUrl(request));
        }
        #endregion

        #region GetPaymentsHistoryUrl
        [Test]
        public void GetPaymentsHistoryUrl_GeneratesUrl()
        {
            var request = new PaymentsHistoryRequest
            {
                CallbackUri = _callBackUri,
                DisplayName = DisplayName
            };

            var actual = _sut.GetPaymentsHistoryUrl(request);

            Assert.IsNotNull(actual);
        }

        [Test]
        public void GetPaymentsHistory_ThrowsWhenNullRequest()
        {
            Assert.Throws<ArgumentException>(() => _sut.GetPaymentsHistoryUrl(null));
        }

        [Test]
        public void GetPaymentsHistory_ThrowsWhenMissingArguments()
        {
            var request = new PaymentsHistoryRequest();

            Assert.Throws<ArgumentNullException>(() => _sut.GetPaymentsHistoryUrl(request));
        }
        #endregion

        #region ValidateResponse
        [Test]
        public void ValidateResponse_ThrowsWhenMissingArguments()
        {
            var responseParameters = new NameValueCollection
            {
                { "fake_param", "fake_value" }
            };

            _httpRequest[Arg.Any<string>()].ReturnsForAnyArgs(x => responseParameters[x.Arg<string>()]);

            Assert.Throws<ArgumentNullException>(() => _sut.ValidateResponse(_httpRequest));
        }

        [Test]
        public void ValidateResponse_ThrowsWhenWrongHash()
        {
            var responseParameters = new NameValueCollection
            {
                { "result", "payment_success" },
                { "timestamp", Timestamp }
            };

            _httpRequest[Arg.Any<string>()].ReturnsForAnyArgs(x => responseParameters[x.Arg<string>()]);

            Assert.Throws<Exception>(() => _sut.ValidateResponse(_httpRequest));
        }

        [Test]
        public void ValidateResponse_ReturnsResponseWhenValid()
        {
            var responseParameters = new NameValueCollection
            {
                {"result", "payment_success"},
                { "description", "fake_description"},
                { "orderId", "fake_order_id" },
                { "errorCode", "100" },
                { "amount", "10" },
                { "tipAmount", "5" },
                { "currency", "eur" },
                { "is_duplicate_receipt", "true" },
                { "payment_method", "fake_payment_method" },
                { "expire_month", "01" },
                { "expire_year", "16" },
                { "effective_month", "03" },
                { "effective_year", "12" },
                { "aid", "fake_aid" },
                { "application_label", "fake_application_label" },
                { "application_preferred_name", "fake_application_preferred_name" },
                { "pan", "fake_pan" },
                { "issuer_identification_number", "fake_issuer_identification_number" },
                { "pan_seq", "fake_pan_seq" },
                { "card_scheme", "fake_card_scheme" },
                { "bank_code", "fake_bank_code" },
                { "pos_entry_mode", "fake_pos_entry_mode" },
                { "merchant_id", "fake_merchant_id" },
                { "merchant_display_name", "fake_merchant_display_name" },
                { "auth_code", "fake_auth_code" },
                { "terminal_id", "fake_terminal_id" },
                { "api_version", "fake_api_version" },
                { "timestamp", Timestamp }
            };
        
            var builder = new UriBuilder("localhost");
            foreach (var key in Consts.HashableResponseKeys)
            {
                builder.AddQuery(key, responseParameters[key].GetValueOrEmpty());
            }

            var hmac = builder.Query
                .Remove(0, 1)
                .ToSha256(Token);

            responseParameters.Add("hmac", hmac);

            _httpRequest[Arg.Any<string>()].ReturnsForAnyArgs(x => responseParameters[x.Arg<string>()]);
            _httpRequest.QueryString.Returns(responseParameters);

            var expected = new PaylevenResponse
            {
                Result = Results.PaymentSuccessful,
                Timestamp = DateTime.Parse(Timestamp),
                Description = "fake_description",
                OrderId = "fake_order_id",
                ErrorCode = ErrorCodes.UnknownError,
                Amount = 10,
                TipAmount = 5,
                Currency = Currencies.EUR,
                IsDuplicateReceipt = true,
                PaymentMethod = "fake_payment_method",
                ExpireMonth = 1,
                ExpireYear = 16,
                EffectiveMonth = 3,
                EffectiveYear = 12,
                Aid = "fake_aid",
                ApplicationLabel = "fake_application_label",
                ApplicationPreferredName = "fake_application_preferred_name",
                Pan = "fake_pan",
                IssuerIdentificationNumber = "fake_issuer_identification_number",
                PanSeq = "fake_pan_seq",
                CardScheme = "fake_card_scheme",
                BankCode = "fake_bank_code",
                PosEntryMode = "fake_pos_entry_mode",
                MerchantId = "fake_merchant_id",
                MerchantDisplayName = "fake_merchant_display_name",
                AuthCode = "fake_auth_code",
                TerminalId = "fake_terminal_id",
                ApiVersion = "fake_api_version"
            };

            var actual = _sut.ValidateResponse(_httpRequest);

            actual.ShouldDeepEqual(expected);
        }
        #endregion
    }
}
