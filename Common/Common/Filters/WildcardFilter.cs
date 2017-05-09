using System;
using System.Linq;
using Ns.Common.FLINQ;

namespace Ns.Common.Filters
{
    public class WildcardFilter : BaseStringFilter
    {
        private readonly string _pattern;

        public WildcardFilter(string pattern)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            _pattern = pattern;
        }

        protected override bool IsMatch(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return IsMatch(value, _pattern);
        }

        private bool IsMatch(string value, string pattern)
        {
            if (value.Is().Not().Given() && pattern.Is().Not().Given())
                return true;
            if (pattern.Any() && pattern.All(c => c == '*'))
                return true;
            if (value.Length < pattern.Length && pattern.Any() && !pattern.Contains('*'))
                return false;
            if (value.Length > pattern.Length && (!pattern.Any() || !pattern.Contains('*')))
                return false;

            var patternChar = pattern.First();

            switch (patternChar)
            {
                case '*':
                    var nextPatternChar = pattern.Cast<char?>().Skip(1).FirstOrDefault();
                    if (!nextPatternChar.HasValue)
                        return true;
                    var index = value.LastIndexOf(nextPatternChar.Value);
                    return index >= 0 && IsMatch(value.Substring(index), pattern.Substring(1));
                case '?':
                    return IsMatch(value.Substring(1), pattern.Substring(1));
                default:
                    return
                        string.Equals(value.First().ToString(), patternChar.ToString(),
                            StringComparison.InvariantCultureIgnoreCase) &&
                        IsMatch(value.Substring(1), pattern.Substring(1));
            }
        }
    }
}