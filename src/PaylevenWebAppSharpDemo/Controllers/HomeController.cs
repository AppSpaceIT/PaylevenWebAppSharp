using System;
using System.Configuration;
using System.Web.Mvc;
using PaylevenWebAppSharp;
using PaylevenWebAppSharp.Enums;

namespace PaylevenWebAppSharpDemo.Controllers
{
    public class HomeController : BaseController
    {
        private readonly string _displayName = ConfigurationManager.AppSettings["DisplayName"];
        private Uri CallBackUri => new Uri(Url.Action("Index", "Response", null, Request.Url.Scheme));

        private ActionResult RedirectOrPrint(string url)
        {
            if (!Request.IsLocal)
            {
                return Redirect(url);
            }

            Response.Write(url);
            Response.End();

            return new EmptyResult();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Refund()
        {
            return View();
        }

        public ActionResult TransactionDetails()
        {
            return View();
        }

        public ActionResult PaymentsHistory()
        {
            var url = PaylevenWebApp.GetPaymentsHistoryUrl(new PaymentsHistoryRequest
            {
                DisplayName = _displayName,            
                CallbackUri = CallBackUri
            });

            return RedirectOrPrint(url);
        }

        public ActionResult DoPayment(string orderId, string description, Currencies? currency, decimal? price)
        {
            if (string.IsNullOrEmpty(orderId) || !price.HasValue || !currency.HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            var url = PaylevenWebApp.GetPaymentUrl(new PaymentRequest
            {
                DisplayName = _displayName,
                OrderId = orderId,
                Description = description,
               // PriceInCents = (int)(price.Value * 100),
                Currency = currency.Value,
                CallbackUri = CallBackUri
            });

            return RedirectOrPrint(url);
        }

        public ActionResult DoRefund(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return RedirectToAction("Refund", "Home");
            }

            var url = PaylevenWebApp.GetRefundUrl(new RefundRequest
            {
                DisplayName = _displayName,
                OrderId = orderId,
                CallbackUri = CallBackUri
            });

            return RedirectOrPrint(url);
        }

        public ActionResult DoTransactionDetails(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return RedirectToAction("TransactionDetails", "Home");
            }

            var url = PaylevenWebApp.GetDetailsUrl(new DetailsRequest
            {                
                DisplayName = _displayName,
                OrderId = orderId,
                CallbackUri = CallBackUri
            });

            return RedirectOrPrint(url);
        }
    }
}