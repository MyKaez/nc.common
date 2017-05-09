namespace Ns.Common.Filters
{
    public interface IFilter<in T>
    {
        bool IsMatch(T value);
    }
}
