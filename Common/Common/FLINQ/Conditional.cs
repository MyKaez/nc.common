using System;
using System.Collections.Generic;
using System.Linq;

namespace Ns.Common.FLINQ
{
    public class Conditional<T>
    {
        private readonly List<Tuple<Predicate<T>, bool>> _predicates;

        public Conditional(Is<T> @is)
        {
            Is = @is;

            _predicates = new List<Tuple<Predicate<T>, bool>>();
        }

        internal Is<T> Is { get; }

        internal IEnumerable<Tuple<Predicate<T>, bool>> Predicates => _predicates;

        public override string ToString()
        {
            return string.Join(" AND ", Predicates.Select(GetPredicateCall));
        }

        private string GetPredicateCall(Tuple<Predicate<T>, bool> predicate)
        {
            var methodInfo = predicate.Item1.Method;
            var methodName = string.Join(string.Empty, methodInfo.Name.Skip(1).TakeWhile(c => c != '>'));

            return (predicate.Item2 ? "NOT " : "") + methodName + "()";
        }

        public static implicit operator bool(Conditional<T> con) => con.Is;

        internal string ToConditionalString() => Is.ToString();

        internal void Add(Predicate<T> predicate)
        {
            _predicates.Add(new Tuple<Predicate<T>, bool>(predicate, Is.Negate));

            if (Is.Negate)
                Is.Negate = false;
        }
    }
}