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
        private static readonly Dictionary<Predicate<string>, Action<IniFile, TextReader>> LineAnalyzers;
        private readonly List<IConfigReader> _children;

        private readonly Dictionary<string, string> _values;

        static IniFile()
        {
            LineAnalyzers = new Dictionary<Predicate<string>, Action<IniFile, TextReader>>
            {
                {l => l.Is().Not().Given(), ThrowAwayLine},
                {l => l.StartsWith(";"), ThrowAwayLine},
                {l => l.StartsWith("@"), HandleList},
                {l => l.StartsWith("[") && l.EndsWith("]"), HandleSection},
                {l => l.Contains("={@"), HandleKeyValuePairBlock},
                {l => l.Contains("="), HandleKeyValuePair}
            };
        }

        public IniFile()
        {
            _values = new Dictionary<string, string>();
            _children = new List<IConfigReader>();

            Parent = null;
        }

        public IniFile(IConfigReader parent) : this()
        {
            Parent = parent;
        }

        public string Section { get; private set; }

        public IEnumerable<IConfigReader> Children => _children;

        public IEnumerable<string> GetEntries() => _values.Keys;

        public string GetString(string key)
        {
            if (_values.ContainsKey(key))
                return _values[key];

            throw new KeyNotFoundException($"The key '{key}' does not exist in the section '{Section}'");
        }

        public IEnumerable<string> GetStrings(string key)
        {
            key = key.StartsWith("@") ? key : "@" + key;

            var keys = _values.Keys.Where(k => k.Is().MatchingRegex($@"{key}\d+"));

            return keys.Select(GetString);
        }

        public IConfigReader Parent { get; }

        public static IniFile Load(string file)
        {
            using (var reader = new StreamReader(file))
            {
                var content = reader.ReadToEnd();

                return Parse(content);
            }
        }

        public static IniFile Parse(string content)
        {
            var ini = new IniFile();

            using (var reader = new StringReader(content))
            {
                var peeker = new LinePeeker(reader);

                while (reader.Peek() >= 0)
                {
                    ini.Handle(peeker);
                }
            }

            return ini;
        }

        private void Handle(LinePeeker peeker)
        {
            var line = peeker.PeekLine() ?? string.Empty;
            var method = LineAnalyzers.FirstOrDefault(a => a.Key(line));

            if (method.Key == null || method.Value == null)
                throw new InvalidOperationException();

            var childInis = _children.OfType<IniFile>().ToArray();
            var ini = childInis.Any() ? childInis.Last() : this;

            method.Value(ini, peeker);
        }

        private static void ThrowAwayLine(IniFile ini, TextReader reader) => reader.ReadLine();

        private static void HandleSection(IniFile ini, TextReader reader)
        {
            var line = reader.ReadLine() ?? string.Empty;
            var section = line.TrimStart("[".ToCharArray()).TrimEnd("]".ToCharArray());

            if (section.Is().Not().Given())
                throw new InvalidOperationException("There is a section without a name in the ini.");


            if (ini.Section.Is().Not().Given())
                ini.Section = section;
            else
            {
                var parent = ini.Parent.Is().Not().Null().And().OfType<IConfigReader, IniFile>()
                    ? ini.Parent.As<IniFile>()
                    : ini;

                var child = new IniFile(parent) {Section = section};

                parent._children.Add(child);
            }
        }

        private static void HandleList(IniFile ini, TextReader reader)
        {
            var line = reader.ReadLine() ?? string.Empty;
            var key = line.Split('=')[0];
            var value = line.Substring(key.Length + 1);
            var entryCount = ini._values.Keys.Count(k => k.Is().MatchingRegex($@"{key}\d+"));

            key = key + entryCount;

            AddKeyValuePair(ini, key, value);
        }

        private static void HandleKeyValuePairBlock(IniFile ini, TextReader reader)
        {
            var line = reader.ReadLine() ?? string.Empty;
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

        private static void HandleKeyValuePair(IniFile ini, TextReader reader)
        {
            var line = reader.ReadLine() ?? string.Empty;
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