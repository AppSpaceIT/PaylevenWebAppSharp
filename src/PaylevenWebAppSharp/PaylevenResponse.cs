using System;
using System.ComponentModel;
using PaylevenWebAppSharp.Enums;

namespace PaylevenWebAppSharp
{
    public class PaylevenResponse
    {            
        public Results Result { get; set; }
        [Description("description")]
        public string Description { get; set; }
        [Description("orderId")]
        public string OrderId { get; set; }
        public ErrorCodes ErrorCode { get; set; }
        [Description("amount")]
        public int Amount { get; set; }
        [Description("tipAmount")]
        public int TipAmount { get; set; }
        public Currencies? Currency { get; set; }
        [Description("is_duplicate_receipt")]
        public bool IsDuplicateReceipt { get; set; }
        [Description("payment_method")]
        public string PaymentMethod { get; set; }
        [Description("effective_month")]
        public short EffectiveMonth { get; set; }
        [Description("effective_year")]
        public short EffectiveYear { get; set; }
        [Description("expire_month")]
        public short ExpireMonth { get; set; }
        [Description("expire_year")]
        public short ExpireYear { get; set; }
        [Description("aid")]
        public string Aid { get; set; }
        [Description("application_label")]
        public string ApplicationLabel { get; set; }
        [Description("application_preferred_name")]
        public string ApplicationPreferredName { get; set; }
        [Description("pan")]
        public string Pan { get; set; }
        [Description("issuer_identification_number")]
        public string IssuerIdentificationNumber { get; set; }
        [Description("pan_seq")]
        public string PanSeq { get; set; }
        [Description("card_scheme")]
        public string CardScheme { get; set; }
        [Description("bank_code")]
        public string BankCode { get; set; }
        [Description("pos_entry_mode")]
        public string PosEntryMode { get; set; }
        [Description("merchant_id")]
        public string MerchantId { get; set; }
        [Description("merchant_display_name")]
        public string MerchantDisplayName { get; set; }
        [Description("auth_code")]
        public string AuthCode { get; set; }
        [Description("terminal_id")]
        public string TerminalId { get; set; }
        [Description("api_version")]
        public string ApiVersion { get; set; }
        [Description("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}