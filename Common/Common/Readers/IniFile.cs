using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ns.Common.FLINQ;

namespace Ns.Common.Readers
{
    public class IniFile : IConfigReader
    {
        private static readonly Dictionary<Predicate<string>, Action<IniFile, string, TextReader>> LineAnalyzers;
        private readonly List<IConfigReader> _children;

        private readonly Dictionary<string, string> _values;

        static IniFile()
        {
            LineAnalyzers = new Dictionary<Predicate<string>, Action<IniFile, string, TextReader>>
            {
                {l => l.StartsWith(";"), (i, l, r) => { }},
                {l => l.Contains("={@"), HandleKeyValuePairBlock},
                {l => l.Contains("="), HandleKeyValuePair}
            };
        }

        public IniFile()
        {
            _values = new Dictionary<string, string>();
            _children = new List<IConfigReader>();
        }

        public string Section { get; private set; }

        public IEnumerable<IConfigReader> Children => _children;

        public IEnumerable<string> GetEntries()
        {
            return _values.Keys;
        }

        public string GetString(string key)
        {
            if (_values.ContainsKey(key))
                return _values[key];

            throw new KeyNotFoundException($"The key '{key}' does not exist in the section '{Section}'");
        }

        public static IniFile Parse(string content)
        {
            var ini = new IniFile();

            using (var reader = new StringReader(content))
            {
                while (reader.Peek() >= 0)
                {
                    ini.Handle(reader);
                }
            }

            return ini;
        }

        private void Handle(TextReader reader)
        {
            var line = reader.ReadLine() ?? string.Empty;

            if (line.Is().Not().Given())
                return;

            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                HandleSection(this, line);
                return;
            }

            var method = LineAnalyzers.FirstOrDefault(a => a.Key(line));

            if (method.Key == null || method.Value == null)
                throw new InvalidOperationException();

            var childInis = _children.OfType<IniFile>().ToArray();
            var ini = childInis.Any() ? childInis.Last() : this;

            method.Value(ini, line, reader);
        }

        private static void HandleSection(IniFile ini, string line)
        {
            var section = line.TrimStart("[".ToCharArray()).TrimEnd("]".ToCharArray());

            if (section.Is().Not().Given())
                throw new InvalidOperationException("There is a section without a name in the ini.");


            if (ini.Section.Is().Not().Given())
                ini.Section = section;
            else
            {
                ini._children.Add(new IniFile {Section = section});
            }
        }

        private static void HandleKeyValuePairBlock(IniFile ini, string line, TextReader reader)
        {
            var key = line.Split('=')[0];
            var builder = new StringBuilder();

            while (reader.Peek() >= 0)
            {
                var l = reader.ReadLine() ?? string.Empty;

                if (l.StartsWith("@}"))
                {
                    AddKeyValuePair(ini, key, builder.ToString());
                    return;
                }

                if (builder.Length != 0)
                    builder.AppendLine();

                builder.Append(l);
            }

            throw new InvalidOperationException(
                $"There is an unclosed block with the key '{key}' in the section '{ini.Section}'");
        }

        private static void HandleKeyValuePair(IniFile ini, string line, TextReader reader)
        {
            var key = line.Split('=')[0];
            var value = line.Substring(key.Length + 1);

            AddKeyValuePair(ini, key, value);
        }

        private static void AddKeyValuePair(IniFile ini, string key, string value)
        {
            if (ini._values.ContainsKey(key))
                throw new InvalidOperationException(
                    $"The key '{key}' exists multiple times in the section '{ini.Section}'");

            ini._values.Add(key, value);
        }
    }
}