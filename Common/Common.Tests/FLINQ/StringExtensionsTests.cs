using System;
using Ns.Common.FLINQ;
using NUnit.Framework;

namespace Ns.Common.Tests.FLINQ
{
    [TestFixture]
    internal class StringExtensionsTests
    {
        [TestCase("", false, true)]
        [TestCase("   ", false, true)]
        [TestCase(null, false, true)]
        [TestCase("value", true, true)]
        [TestCase("", false, false)]
        [TestCase("   ", true, false)]
        [TestCase(null, false, false)]
        [TestCase("value", true, false)]
        public void IsGiven_ValueGiven(string value, bool isGiven, bool ignoreWhitespaces)
        {
            bool con = value.Is().Given(ignoreWhitespaces);

            Assert.AreEqual(con, isGiven);
        }

        [TestCase("value", ".*", true)]
        [TestCase("value", @"\d", false)]
        public void MatchingRegex_ValidInputGiven(string value, string pattern, bool matching)
        {
            bool con = value.Is().MatchingRegex(pattern);

            Assert.AreEqual(matching, con);
        }

        [TestCase(null, "", typeof(ArgumentNullException))]
        [TestCase("", null, typeof(ArgumentNullException))]
        [TestCase(null, null, typeof(ArgumentNullException))]
        [TestCase("", "[", typeof(ArgumentException))]
        public void MatchingRegex_NoValidInputGiven(string value, string pattern, Type exceptionType)
        {
            Assert.Throws(exceptionType, () =>
            {
                bool con = value.Is().MatchingRegex(pattern);

                Assert.Fail("An exception should have been thrown, instead a result was evaluated: " + con);
            });
        }

        [TestCase("value", "*", true)]
        [TestCase("value", "*ue", true)]
        [TestCase("value", "val*", true)]
        [TestCase("value", "VALue", true)]
        [TestCase("value", "*value", true)]
        [TestCase("value", "value*", true)]
        [TestCase("value", "*value*", true)]
        [TestCase("value", "", false)]
        [TestCase("value", "***", true)]
        [TestCase("value", "Z*", false)]
        [TestCase("value", "*Z", false)]
        [TestCase("v", "?", true)]
        [TestCase("", "?", false)]
        public void MatchingWildcard_ValidInputGiven(string value, string pattern, bool matching)
        {
            bool con = value.Is().MatchingWildcard(pattern);

            Assert.AreEqual(matching, con);
        }

        [TestCase(null, "")]
        [TestCase("", null)]
        [TestCase(null, null)]
        public void MatchingWildcard_NoValidInputGiven(string value, string pattern)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                bool con = value.Is().MatchingWildcard(pattern);

                Assert.Fail("An exception should have been thrown, instead a result was evaluated: " + con);
            });
        }

        [TestCase("value", "value", true)]
        [TestCase("value", "VALUE", true)]
        [TestCase("value", "", false)]
        [TestCase("", "value", false)]
        public void Matching_ValidInputGiven(string value, string pattern, bool matching)
        {
            bool con = value.Is().Matching(pattern);

            Assert.AreEqual(matching, con);
        }

        [TestCase("", null)]
        [TestCase(null, "")]
        [TestCase(null, null)]
        public void Matching_NoValidInputGiven(string value, string pattern)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                bool con = value.Is().Matching(pattern);

                Assert.Fail("An exception should have been thrown, instead a result was evaluated: " + con);
            });
        }
    }
}