namespace PaylevenWebAppSharp.Enums
{
    public enum ErrorCodes
    {
        NoError = 0,
        UnknownError = 100,
        PaymentAlreadyExists = 101,
        SignatureInvalid = 102,
        ApiKeyNotFound = 103,
        ApiServiceFailed = 104,
        ApiServiceError = 105,
        CountryInvalid = 106,
        FailedPaymentAuth = 107,
        TransactionNotFound = 108,
        FailedRefund = 109,
        TerminalMisconfigured = 110,
        NetworkConnectionTimedOut = 111,
        AlreadyRefunded = 112,
        PaymentWasCanceled = 113,
        PaymentWasDeclined = 114,
        AnotherTransactionInProgress = 115
    }
}