using System;
using System.Linq;
using System.Text;

namespace Ns.Common.FLINQ
{
    public class Conditional<T>
    {
        internal Conditional(Is<T> @is, Predicate<T> predicate, T value)
        {
            Negate = @is.Negate;

            Is = @is;
            Predicate = predicate;
            Value = value;
        }

        internal Is<T> Is { get; }

        internal Predicate<T> Predicate { get; }

        internal T Value { get; }

        internal bool Negate { get; set; }

        public static implicit operator bool(Conditional<T> conditional)
        {
            return conditional.Is;
        }

        public override string ToString()
        {
            return @Is.ToString();
        }

        internal string ToConditionalString()
        {
            return $"is {GetPredicateCall()}";
        }

        private string GetPredicateCall()
        {
            var methodInfo = Predicate.Method;
            var methodName = string.Join(string.Empty, methodInfo.Name.Skip(1).TakeWhile(c => c != '>'));
            var builder = new StringBuilder();

            foreach (var c in methodName)
            {
                if (char.IsUpper(c))
                {
                    if (builder.Length != 0)
                        builder.Append(' ');

                    builder.Append(char.ToLower(c));
                }
                else
                {
                    builder.Append(c);
                }
            }

            return (Negate ? "not " : "") + $@"{builder} [ ""{Value}"" ]";
        }

        internal bool IsOk()
        {
            var result = Predicate(Value);

            if (Negate)
                return !result;

            return result;
        }
    }
}