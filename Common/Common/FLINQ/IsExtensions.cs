using System;
using System.Linq;

namespace Ns.Common.FLINQ
{
    public static class IsExtensions
    {
        public static Conditional<T> CreateConditional<T>(this Is<T> @is, Predicate<T> predicate)
        {
            if (!@is.Conditionals.Any())
                @is.Conditionals.Add(new Conditional<T>(@is));

            var conditional = @is.Conditionals.Last();

            conditional.Add(predicate);

            return conditional;
        }

        public static Is<T> Not<T>(this Is<T> @is)
        {
            @is.Negate = true;

            return @is;
        }

        public static Is<T> And<T>(this Conditional<T> conditional) => conditional.Is;

        public static Is<T> Or<T>(this Conditional<T> conditional)
        {
            conditional.Is.Conditionals.Add(new Conditional<T>(conditional.Is));

            return conditional.Is;
        }
    }
}