using System.ComponentModel;

namespace PaylevenWebAppSharp.Enums
{
    public enum Results
    {
        [Description("payment_success")] PaymentSuccessful,
        [Description("payment_error")] PaymentError,
        [Description("payment_canceled")] PaymentCanceled,
        [Description("refund_success")] RefundSuccessful,
        [Description("refund_error")] RefundError,
        [Description("refund_canceled")] RefundCanceled,
        [Description("transaction_not_found")] TransactionNotFound,
        [Description("sales_history_cancel")] SalesHistoryCanceled,
        [Description("login_canceled")] LoginCanceled,
        [Description("trxdetail_canceled")] TransactionDetailsCanceled
    }
}