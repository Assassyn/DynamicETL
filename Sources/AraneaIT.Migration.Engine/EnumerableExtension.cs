// <copyright file="EnumerableExtension.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Adds new functionality to IEnumerable
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// Iterates the specified collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="iterationAction">The iteration action.</param>
        /// <returns>Enumeration of provided action</returns>
        public static IEnumerable<T> Iterate<T>(this IEnumerable<T> collection, Func<T, T> iterationAction)
        {
            foreach (var item in collection)
            {
                yield return iterationAction(item);
            }
        }
    }
}