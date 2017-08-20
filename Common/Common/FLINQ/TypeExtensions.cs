using System;

namespace Ns.Common.FLINQ
{
    public static class TypeExtensions
    {
        public static Conditional<Type> Nullable(this Is<Type> @is)
            =>
                @is.CreateConditional(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof (Nullable<>),
                    @is.Value);

        public static Conditional<Type> OfType<T>(this Is<Type> @is)
            => @is.CreateConditional(t => t == typeof (T), @is.Value);
    }
}