using System;
using System.Collections.Generic;
using System.Linq;

namespace ScrapEra.CleanReader
{
    public static class TraversalExtension
    {
        public static T SingleOrNone<T>(this IEnumerable<T> enumerable)
            where T : class
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }
            var firstElement = enumerable.FirstOrDefault();
            if (firstElement == null)
            {
                return null;
            }
            var secondElement = enumerable.Skip(1).FirstOrDefault();
            if (secondElement != null)
            {
                return null;
            }
            return firstElement;
        }
    }
}