using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace Flowdesks.Application.Extensions
{
    public static class DateTimeExtensions
    {
        public static T ConvertUtcDateTimePropertiesToLocalTime<T>(this T obj, HttpContext httpContext)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    var utcDateTime = property.GetValue(obj) as DateTime?;
                    if (utcDateTime.HasValue)
                    {
                        var localDateTime = utcDateTime.Value.ToUserLocalTime(httpContext);
                        property.SetValue(obj, localDateTime);
                    }
                }
            }
            return obj;
        }

        public static DateTime ToUserLocalTime(this DateTime utcDateTime, HttpContext httpContext)
        {
            if (httpContext != null &&
                httpContext.Items.ContainsKey("TimezoneOffset") &&
                httpContext.Items["TimezoneOffset"] is string timezoneOffsetString &&
                double.TryParse(timezoneOffsetString, out double timezoneOffset))
            {
                var offset = TimeSpan.FromHours(timezoneOffset);
                var localDateTime = utcDateTime.Add(offset);
                return DateTime.SpecifyKind(localDateTime, DateTimeKind.Local);
            }
            return utcDateTime;
        }

    }
}