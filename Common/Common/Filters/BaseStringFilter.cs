namespace Ns.Common.Filters
{
    public abstract class BaseStringFilter : IFilter<string>
    {
        bool IFilter<string>.IsMatch(string value)
        {
            return IsMatch(value);
        }

        protected abstract bool IsMatch(string value);
    }
}