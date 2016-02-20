using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PaylevenWebAppSharp;

namespace PaylevenWebAppSharpDemo.Controllers
{
    public class ResponseController : BaseController
    {
        public ActionResult Index()
        {
            try
            {
                var response = PaylevenWebApp.ValidateResponse(Request);

                var type = typeof(PaylevenResponse);

                var properties = type
                    .GetProperties()
                    .ToDictionary(property => property.Name, property => property.GetValue(response));

                properties.Add("Query", Request.QueryString);

                return View(properties);
            }
            catch (Exception ex)
            {
                return View(new Dictionary<string, object>
                {
                    { "Exception", ex.Message },
                    { "Query", Request.QueryString }
                });
            }
        }
    }
}