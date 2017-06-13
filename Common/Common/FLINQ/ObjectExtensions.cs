namespace Ns.Common.FLINQ
{
    public static class ObjectExtensions
    {
        public static T As<T>(this object value) where T : class => value as T;

        public static Is<T> Is<T>(this T value) => new Is<T>(value);
    }
}