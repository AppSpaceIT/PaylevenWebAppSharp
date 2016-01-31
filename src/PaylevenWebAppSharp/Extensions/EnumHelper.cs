using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace PaylevenWebAppSharp.Extensions
{
    public static class EnumHelper<T>
    {
        public static T ParseEnum(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        private static T GetValueFromDescription(string description, bool useDefault)
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }

            if (useDefault)
            {
                return default(T);
            }

            throw new ArgumentException("Not found.", "description");
        }

        public static T GetValueFromDescription(string description)
        {
            return GetValueFromDescription(description, false);
        }

        public static T GetValueFromDescriptionOrDefault(string description)
        {
            return GetValueFromDescription(description, true);
        }
    }
}
