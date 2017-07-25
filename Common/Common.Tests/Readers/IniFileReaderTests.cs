using System;
using System.Linq;
using Ns.Common.FLINQ;
using Ns.Common.Readers;
using NUnit.Framework;

namespace Ns.Common.Tests.Readers
{
    [TestFixture]
    internal class IniFileReaderTests
    {
        [TestCase]
        public void Parse_SimpleSection()
        {
            const string content = @"
[Option]
Key1=Value
Key2=Value
Key3=Value
";
            var ini = IniFile.Parse(content);

            Assert.AreEqual("Option", ini.Section);
            Assert.AreEqual(3, ini.GetEntries().Count());
        }

        [TestCase]
        public void Parse_SameKeyMultipleTimes()
        {
            const string content = @"
[Option]
Key1=Value
Key1=Value
";

            Assert.Throws<InvalidOperationException>(() => IniFile.Parse(content));
        }

        [TestCase]
        public void Parse_SameKeyIsInComment()
        {
            const string content = @"
[Option]
;Key1=Value
Key1=Value
";

            var ini = IniFile.Parse(content);

            Assert.AreEqual(1, ini.GetEntries().Count());
        }

        [TestCase]
        public void Parse_SectionNameIsNotGiven()
        {
            const string content = @"
[]
;Key1=Value
Key1=Value
";

            Assert.Throws<InvalidOperationException>(() => IniFile.Parse(content));
        }

        [TestCase]
        public void Parse_FurtherSectionsAreParsedAsChildren()
        {
            const string content = @"
[Option1]
Key=Value1
[Option2]
Key=Value2
[Option3]
Key=Value3
[Option4]
Key=Value4
";

            var ini = IniFile.Parse(content);

            Assert.AreEqual("Option1", ini.Section);
            Assert.AreEqual(3, ini.Children.Count());

            for (var i = 2; i <= 4; i++)
                Assert.AreEqual("Option" + i, ini.Children.Get(i - 2).Section);
        }

        [TestCase]
        public void Parse_BlockCanBeRead()
        {
            const string value = @"This is quite a long text
Which can be stored in a nice variable";
            const string content = @"
[Option]
Key={@
" + value + @"
@}
";

            var ini = IniFile.Parse(content);

            Assert.AreEqual(value, ini.GetString("Key"));
        }

        [TestCase]
        public void Parse_UnclosedBlockCanNotBeRead()
        {
            const string value = @"This is quite a long text
Which can be stored in a nice variable";
            const string content = @"
[Option]
Key={@
" + value;

            Assert.Throws<InvalidOperationException>(() => IniFile.Parse(content));
        }

        [TestCase]
        public void GetString_ChildrenHaveSameKeysWithOwnValue()
        {
            const string content = @"
[Option1]
Key=Value1
[Option2]
Key=Value2
[Option3]
Key=Value3
[Option4]
Key=Value4
";

            var ini = IniFile.Parse(content);

            for (var i = 2; i <= 4; i++)
            {
                var child = ini.Children.Get(i - 2);
                var value = child.GetString("Key");

                Assert.AreEqual("Value" + i, value);
            }
        }

        [TestCase]
        public void GetString_ReturnedCorrectValue()
        {
            const string content = @"
[Option]
Key1=Value
Key2=Value
Key3=Value
";

            var ini = IniFile.Parse(content);

            foreach (var entry in ini.GetEntries())
                Assert.AreEqual("Value", ini.GetString(entry));
        }

        [TestCase]
        public void GetString_NotCommentedValueTaken()
        {
            const string content = @"
[Option]
;Key=Value1
;Key=Value2
Key=Value3
";

            var ini = IniFile.Parse(content);

            foreach (var entry in ini.GetEntries())
                Assert.AreEqual("Value3", ini.GetString(entry));
        }

        [TestCase]
        public void GetStrings_ListOfStringsCanBeRead()
        {
            const string content = @"
[Option]
@Key=Val0
@Key=Val1
@Key=Val2
";

            var ini = IniFile.Parse(content);
            var values = ini.GetStrings("Key").ToArray();

            Assert.AreEqual(3, values.Length);

            for (var i = 0; i < values.Length; i++)
                Assert.AreEqual("Val" + i, values[i]);
        }

        [TestCase]
        public void GetStrings_ListOfStringsAndSingleStringWithSameKey()
        {
            const string content = @"
[Option]
Key=Val
@Key=Val0
@Key=Val1
@Key=Val2
";

            var ini = IniFile.Parse(content);
            var values = ini.GetStrings("Key").ToArray();

            Assert.AreEqual(3, values.Length);

            for (var i = 0; i < values.Length; i++)
                Assert.AreEqual("Val" + i, values[i]);

            Assert.AreEqual("Val", ini.GetString("Key"));
        }
    }
}