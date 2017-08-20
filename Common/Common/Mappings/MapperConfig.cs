using System.Collections.Generic;

namespace Ns.Common.Mappings
{
    public class MapperConfig
    {
        private readonly List<Mapping> _mappings = new List<Mapping>();

        public IEnumerable<Mapping> Mappings => _mappings;

        public void Add(Mapping mapping) => _mappings.Add(mapping);
    }
}