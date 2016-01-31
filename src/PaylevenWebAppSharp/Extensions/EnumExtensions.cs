using System;
using System.ComponentModel;
using System.Linq;

namespace PaylevenWebAppSharp.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }

            var field = type.GetField(name);
            if (field == null)
            {
                return null;
            }

            var attr = Attribute.GetCustomAttribute(field, typeof (DescriptionAttribute)) as DescriptionAttribute;
            return attr != null
                ? attr.Description
                : null;
        }

        public static T GetEnumValueFromDescription<T>(this string description)
        {
            var type = typeof (T);
            if (!type.IsEnum)
            {
                throw new ArgumentException();
            }

            var fields = type.GetFields();
            var field = fields
                .SelectMany(f =>
                    f.GetCustomAttributes(typeof (DescriptionAttribute), false), (f, a) => new {Field = f, Att = a})
                .SingleOrDefault(a => ((DescriptionAttribute) a.Att).Description == description);

            return field == null
                ? default(T)
                : (T) field.Field.GetRawConstantValue();
        }
    }
}