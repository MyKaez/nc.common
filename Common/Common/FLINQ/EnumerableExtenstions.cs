using System.Collections;
using System.Text;

namespace Ns.Common.FLINQ
{
    public static class EnumerableExtenstions
    {
        public static string JoinToString(this IEnumerable enumerable, string join = ", ", string enclosure = "")
        {
            var builder = new StringBuilder();

            foreach (var item in enumerable)
            {
                if (builder.Length > 0)
                    builder.Append(join);

                var itemString = item.ToString();

                if (itemString.Is().Given())
                {
                    if (itemString.Contains(enclosure))
                        itemString = $"{enclosure}{itemString}{enclosure}";
                }

                builder.Append(itemString);
            }

            return builder.ToString();
        }
    }
}