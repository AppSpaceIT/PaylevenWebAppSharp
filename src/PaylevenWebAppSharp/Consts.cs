namespace PaylevenWebAppSharp
{
    public static class Consts
    {
        public const string BaseUrl = "paylevenweb://payleven";
        public const string Version = "1.2";

        public const string DefaultToken = "vKqmpnwrad3N";
       
        public static readonly string[] HashableResponseKeys =
        {
            "result",
            "description",
            "orderId",
            "errorCode",
            "amount",
            "tipAmount",
            "currency",
            "is_duplicate_receipt",
            "payment_method",
            "expire_month",
            "expire_year",
            "effective_month",
            "effective_year",
            "aid",
            "application_label",
            "application_preferred_name",
            "pan",
            "issuer_identification_number",
            "pan_seq",
            "card_scheme",
            "bank_code",
            "pos_entry_mode",
            "merchant_id",
            "merchant_display_name",
            "auth_code",
            "terminal_id",
            "api_version",
            "timestamp"
        };
    };
}