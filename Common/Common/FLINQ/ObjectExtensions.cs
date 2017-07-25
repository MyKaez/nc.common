namespace Ns.Common.FLINQ
{
    public static class ObjectExtensions
    {
        public static T As<T>(this object value) where T : class => value as T;

        public static Is<T> Is<T>(this T value) => new Is<T>(value);

        public static T Convert<T>(this object value)
        {
            if (value.Is().Null())
                return default(T);

            if (value.GetType() == typeof (T))
                return (T) value;

            return (T) System.Convert.ChangeType(value, typeof (T));
        }
    }
}