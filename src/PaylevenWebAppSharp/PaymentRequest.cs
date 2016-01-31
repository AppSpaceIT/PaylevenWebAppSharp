using System;
using PaylevenWebAppSharp.Enums;

namespace PaylevenWebAppSharp
{
    public class PaymentRequest : BaseOrderRequest
    {
        public int PriceInCents { get; set; }
        public Currencies Currency { get; set; }
        public string Description { get; set; }
    }
}