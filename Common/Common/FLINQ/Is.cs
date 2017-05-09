using System;
using System.Collections.Generic;
using System.Linq;

namespace Ns.Common.FLINQ
{
    public class Is<T>
    {
        internal Is(T value)
        {
            Value = value;
            Conditionals = new List<Conditional<T>>();
        }

        internal T Value { get; }

        internal ICollection<Conditional<T>> Conditionals { get; }

        internal bool Negate { get; set; }

        public static implicit operator bool(Is<T> @is)
        {
            return @is.Conditionals.Any(conditional => conditional.Predicates.All(p => p(@is.Value)));
        }

        public override string ToString()
        {
            var list = new List<string>();

            list.AddRange(Conditionals.Select(c => c.ToString()));

            return $"Check on '{Value}': " + Environment.NewLine +
                   string.Join(Environment.NewLine, list.Select((l, i) => $"{i + 1}: {l}"));
        }

        internal void NegateNextPredicate()
        {
            Negate = true;
        }
    }
}