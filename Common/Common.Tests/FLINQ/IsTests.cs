using Ns.Common.FLINQ;
using NUnit.Framework;

namespace Ns.Common.Tests.FLINQ
{
    [TestFixture]
    public class IsTests
    {
        [TestCase]
        public void Not_ConditionIsNegated()
        {
            var con = "".Is().Not().Given();

            Assert.True(con);
        }

        [TestCase]
        public void ToString_MethodsAreNamed()
        {
            var con = "value".Is().Given().And().Not().MatchingRegex(".*").Or().MatchingWildcard("*");
            var conString = con.ToString();

            Assert.True(con);
            Assert.True(conString.Contains("given"));
            Assert.True(conString.Contains("and"));
            Assert.True(conString.Contains("not"));
            Assert.True(conString.Contains("matching regex"));
            Assert.True(conString.Contains("or"));
            Assert.True(conString.Contains("matching wildcard"));
        }
    }
}
