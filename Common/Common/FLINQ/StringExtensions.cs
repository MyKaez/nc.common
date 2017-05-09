﻿using System;
using Ns.Common.Filters;

namespace Ns.Common.FLINQ
{
    public static class StringExtensions
    {
        public static Conditional<string> Given(this Is<string> @is, bool ignoreWhitespaces = true)
            => @is.CreateConditional(v => ignoreWhitespaces ? !string.IsNullOrWhiteSpace(v) : !string.IsNullOrEmpty(v));

        public static Conditional<string> MatchingRegex(this Is<string> @is, string pattern)
            => @is.CreateConditional(v => pattern.CreateFilter(StringFilterType.Regex).IsMatch(v));

        public static Conditional<string> MatchingWildcard(this Is<string> @is, string pattern)
            => @is.CreateConditional(v => pattern.CreateFilter(StringFilterType.Wildcard).IsMatch(v));

        public static IFilter<string> CreateFilter(this string pattern, StringFilterType filterType)
        {
            switch (filterType)
            {
                case StringFilterType.Regex:
                    return new RegexFilter(pattern);
                case StringFilterType.Wildcard:
                    return new WildcardFilter(pattern);
                default:
                    throw new InvalidCastException($"The filter type '{filterType}' is not mapped yet.");
            }
        }
    }
}