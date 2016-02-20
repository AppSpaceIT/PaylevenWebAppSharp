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

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            PaylevenWebApp = new PaylevenWebApp(token);
        }
    }
}