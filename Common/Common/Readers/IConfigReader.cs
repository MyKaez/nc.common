using System.Collections.Generic;

namespace Ns.Common.Readers
{
    public interface IConfigReader
    {
        string Section { get; }

        IEnumerable<IConfigReader> Children { get; }

        IEnumerable<string> GetEntries();

        string GetString(string key);

        IEnumerable<string> GetStrings(string key);

        IConfigReader Parent { get; }
    }
}