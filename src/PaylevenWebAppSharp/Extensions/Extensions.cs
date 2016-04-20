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
            nvc.Add(name, value ?? string.Empty);
            uriBuilder.Query = nvc.ToString();

            return uriBuilder;
        }

        public static string ToSha256(this string data, string secret)
        {
            var key = Encoding.UTF8.GetBytes(secret);
            var message = Encoding.UTF8.GetBytes(data);
            var hashString = new HMACSHA256(key);

            var hashValue = hashString.ComputeHash(message);
            return hashValue.Aggregate("", (current, x) => current + $"{x:x2}");
        }

        public static UriBuilder ComputeAndAppendSha256ToUri(this UriBuilder uriBuilder, string token)
        {
            uriBuilder.Path += uriBuilder.Query
                .TrimStart('?')
                .ToSha256(token);

            return uriBuilder;
        }

        public static string GetValueOrEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value) || value == "0"
                ? string.Empty
                : value;
        }

        public static DateTime ToDateTime(this long value)
        {
            var dateTime = new DateTimeOffset(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            dateTime = dateTime.AddMilliseconds(value);

            return dateTime.UtcDateTime;
        }
    }
}