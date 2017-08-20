namespace Ns.Common.Mappings
{
    public class Mapping
    {
        public Mapping(MappingEntity source, MappingEntity target)
        {
            Source = source;
            Target = target;
        }

        public MappingEntity Source { get; }

        public MappingEntity Target { get; }

        public string ConversionFormat { get; set; }
    }
}