using System;
using System.Linq;
using System.Web.Mvc;
using PaylevenWebAppSharp;

namespace PaylevenWebAppSharpDemo.Controllers
{
    public class ResponseController : BaseController
    {
        public ActionResult Index()
        {
            var response = PaylevenWebApp.ValidateResponse(Request);

            var type = typeof(PaylevenResponse);

            var properties = type
                .GetProperties()
                .ToDictionary(property => property.Name, property => property.GetValue(response));

            return View(properties);
        }
    }
}