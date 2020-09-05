using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Payments.Model
{
    static class Extensions
    {
        public static IEnumerable<T> ToIEnumerable<T>(this T instance)
        {
            return new List<T>(1) { instance };
        }
        public static int FindIndex<T>(this IList<T> source, Predicate<T> match)
        {
            return source.FindIndex(0, match);
        }
        public static int FindIndex<T>(this IList<T> source, int startIndex,
                               Predicate<T> match)
        {
            // TODO: Validation
            for (int i = startIndex; i < source.Count; i++)
            {
                if (match(source[i]))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
