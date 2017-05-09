using System.Text.RegularExpressions;

namespace Ns.Common.Filters
{
    public class RegexFilter : BaseStringFilter
    {
        private readonly Regex _regex;

        public RegexFilter(string pattern)
        {
            _regex = new Regex(pattern, RegexOptions.Compiled);
        }

        protected override bool IsMatch(string value)
        {
            return _regex.IsMatch(value);
        }
    }
}