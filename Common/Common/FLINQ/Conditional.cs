using System;
using System.Collections.Generic;
using System.Linq;

namespace Ns.Common.FLINQ
{
    public class Conditional<T>
    {
        private readonly List<Predicate<T>> _predicates;

        public Conditional(Is<T> @is)
        {
            Is = @is;

            _predicates = new List<Predicate<T>>();
        }

        internal Is<T> Is { get; }

        internal IEnumerable<Predicate<T>> Predicates => _predicates;

        public override string ToString()
        {
            return string.Join(" AND ", Predicates.Select(GetPredicateCall));
        }

        private string GetPredicateCall(Predicate<T> predicate)
        {
            var methodInfo = predicate.Method;
            var methodName = string.Join(string.Empty, methodInfo.Name.Skip(1).TakeWhile(c => c != '>'));

            return methodName + "()";
        }

        public static implicit operator bool(Conditional<T> con) => con.Is;

        internal string ToConditionalString() => Is.ToString();

        internal void Add(Predicate<T> predicate)
        {
            if (Is.Negate)
            {
                _predicates.Add(v => !predicate(v));

                Is.Negate = false;
            }
            else
                _predicates.Add(predicate);
        }
    }
}