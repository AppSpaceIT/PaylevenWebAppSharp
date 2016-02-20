using System;
using System.Configuration;
using System.Web.Mvc;
using PaylevenWebAppSharp;

namespace PaylevenWebAppSharpDemo.Controllers
{
    public class BaseController : Controller
    {
        private const string PaylevenToken = "PaylevenToken";
        internal readonly PaylevenWebApp PaylevenWebApp;

        public BaseController()
        {
            var token = ConfigurationManager.AppSettings[PaylevenToken] ??
                        Environment.GetEnvironmentVariable(PaylevenToken, EnvironmentVariableTarget.Machine);

            PaylevenWebApp = new PaylevenWebApp(token);
        }
    }
}