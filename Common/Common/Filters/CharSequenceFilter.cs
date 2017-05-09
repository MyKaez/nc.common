using System;

namespace Ns.Common.Filters
{
    public class CharSequenceFilter : BaseStringFilter
    {
        private readonly string _pattern;

        public CharSequenceFilter(string pattern)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            _pattern = pattern;
        }

        protected override bool IsMatch(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return _pattern.Equals(value, StringComparison.OrdinalIgnoreCase);
        }
    }
}