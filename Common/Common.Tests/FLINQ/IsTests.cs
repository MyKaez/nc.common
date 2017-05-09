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
            var con = "value".Is().Given().And().Not().MatchingRegex(".*");
            var conString = con.ToString();

            Assert.True(conString.Contains("Given"));
            Assert.True(conString.Contains("AND"));
            Assert.True(conString.Contains("NOT"));
            Assert.True(conString.Contains("MatchingRegex"));
        }
    }
}
