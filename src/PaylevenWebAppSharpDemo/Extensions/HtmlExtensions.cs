using System;
using System.Web.Mvc;

namespace PaylevenWebAppSharpDemo.Extensions
{
    public static class HtmlEtensions
    {
        public static string IsActive(this HtmlHelper html, string action, string controller)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeController = (string)routeData.Values["controller"];

            var returnActive = string.Equals(controller, routeController, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(action, routeAction, StringComparison.InvariantCultureIgnoreCase);

            return returnActive ? "active" : "";
        }
    }
}