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
            Conditionals = new List<List<Conditional<T>>>();
        }

        internal T Value { get; }

        internal List<List<Conditional<T>>> Conditionals { get; set; }

        internal bool Negate { get; set; }

        public static implicit operator bool(Is<T> @is)
        {
            foreach(var con in @is.Conditionals)
                if (con.Any() && con.All(c => c.IsOk()))
                    return true;

            return false;
        }

        public override string ToString()
        {
            var list = Conditionals.Select(con => con.Select(c => c.ToConditionalString()).JoinToString(" and "));
            var checkString = $"Check on '{Value}': " + Environment.NewLine +
                              string.Join(Environment.NewLine, list.Select((l, i) => $"{i + 1}: {l}"));

            return checkString;
        }
    }
}