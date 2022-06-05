#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;

namespace Rosalina.Extensions
{
    internal static class LinqExtensions
    {
        public static IEnumerable<TValue> FlattenTree<TValue>(this IEnumerable<TValue> source, Func<TValue, IEnumerable<TValue>> selector)
        {
            return source.SelectMany(x => selector(x).FlattenTree(selector)).Concat(source);
        }
    }
}

#endif