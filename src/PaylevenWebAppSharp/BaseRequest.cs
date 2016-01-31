using System;

namespace PaylevenWebAppSharp
{
    public abstract class BaseRequest
    {
        public Uri CallbackUri { get; set; }
        public string DisplayName { get; set; }
    }
}