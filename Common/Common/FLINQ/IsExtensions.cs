using System;
using System.Collections.Generic;
using System.Linq;

namespace Ns.Common.FLINQ
{
    public static class IsExtensions
    {
        public static Conditional<T> CreateConditional<T>(this Is<T> @is, Predicate<T> predicate, T value)
        {
            var conditional = new Conditional<T>(@is, predicate, value);

            if (!@is.Conditionals.Any())
                @is.Conditionals.Add(new List<Conditional<T>>());
            
            @is.Conditionals.Last().Add(conditional);

            return conditional;
        }

        public static Is<T> Not<T>(this Is<T> @is)
        {
            @is.Negate = !@is.Negate;
            
            return @is;
        }

        public static Is<T> And<T>(this Conditional<T> conditional) => conditional.Is;

        public static Is<T> Or<T>(this Conditional<T> conditional)
        {
            conditional.Is.Negate = false;
            conditional.Is.Conditionals.Add(new List<Conditional<T>>());

            return conditional.Is;
        }

        public static Conditional<T> Null<T>(this Is<T> @is) => @is.CreateConditional(p => p == null, @is.Value);

        public static Conditional<T> OfType<T>(this Is<T> @is) => @is.CreateConditional(p => p.GetType() == typeof(T), @is.Value);
    }
}