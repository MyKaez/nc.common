using System;
using System.Globalization;

namespace Ns.Common.FLINQ
{
    public static class ObjectExtensions
    {
        public static T As<T>(this object value) where T : class => value as T;

        public static Is<T> Is<T>(this T value) => new Is<T>(value);

        public static T Convert<T>(this object value, string format = null)
        {
            return (T) value.Convert(typeof (T), format);
        }

        public static object Convert(this object value, Type type, string format = null)
        {
            if (value.Is().Null())
                return null;

            if (type.Is().Nullable())
                return value.Convert(Nullable.GetUnderlyingType(type), format);

            if (value.GetType() == type && format.Is().Not().Given())
                return value;

            if (format.Is().Not().Given())
                return System.Convert.ChangeType(value, type);

            if (type.IsClass && type.Is().Not().OfType<string>())
                throw new InvalidOperationException(
                    "Only certain types allow to be formatted (numbers, date times, strings)");

            var stringValue = value.Convert<string>();
            format = format ?? string.Empty;

            switch (type.Name)
            {
                case nameof(DateTime):
                    return DateTime.ParseExact(stringValue, format, CultureInfo.InvariantCulture);
                case nameof(String):
                    return format.Replace("$VALUE$", stringValue);
                default:
                    throw new InvalidOperationException(
                        "Only certain types allow to be formatted (numbers, date times, strings)");
            }
        }
    }
}