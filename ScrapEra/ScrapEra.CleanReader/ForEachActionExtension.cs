using System;
using System.Collections.Generic;

namespace ScrapEra.CleanReader
{
    public static class ForEachActionExtension
    {
        public static bool IsCloseToZero(this float x)
        {
            return Math.Abs(x) < float.Epsilon;
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var element in enumerable)
            {
                action(element);
            }
        }
    }
}