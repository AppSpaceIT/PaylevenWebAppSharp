using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PaylevenWebAppSharp.Extensions
{
    public static class Extensions
    {
        public static UriBuilder AddQuery(this UriBuilder uriBuilder, string name, string value)
        {
            var nvc = HttpUtility.ParseQueryString(uriBuilder.Query);
            nvc.Add(name, HttpUtility.UrlEncode(value) ?? string.Empty);
            uriBuilder.Query = nvc.ToString();

            return uriBuilder;
        }

        public static string ToSha256(this string secret, string data)
        {
            var key = Encoding.ASCII.GetBytes(secret);
            var message = Encoding.ASCII.GetBytes(data);
            var hashString = new HMACSHA256(key);

            var hashValue = hashString.ComputeHash(message);
            return hashValue.Aggregate("", (current, x) => current + string.Format((string) "{0:x2}", (object) x));
        }

        public static UriBuilder ComputeAndAppendSha256ToUri(this UriBuilder uriBuilder, string token)
        {
            uriBuilder.Path += uriBuilder.Query
                .Remove(0, 1) // remove the ? from the query part
                .ToSha256(token);

            return uriBuilder;
        }

        public static string GetValueOrEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value) || value == "0"
                ? string.Empty
                : value;
        }
    }
}