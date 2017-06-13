using System.IO;
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

            Assert.True(conString.Contains("given"));
            Assert.True(conString.Contains("and"));
            Assert.True(conString.Contains("not"));
            Assert.True(conString.Contains("matching regex"));
        }

        [TestCase]
        public void ToString_OrSeparatesConditions()
        {
            var con = "".Is().Null().Or().Matching("*");
            var conString = con.ToString();
            var count = 0;

            using (var reader = new StringReader(conString))
            {
                while (reader.ReadLine() != null)
                    count++;
            }

            Assert.True(count >= 2);
        }
    }
}
