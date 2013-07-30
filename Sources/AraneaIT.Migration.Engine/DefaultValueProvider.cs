// <copyright file="DefaultValueProvider.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using System.Collections.Generic;

    public static class DefaultValueProvider
    {
        public static TValue GetValueOrDefault<TValue>(this IDictionary<string, TValue> set, string key, TValue defaultValue)
        {
            return set.ContainsKey(key) ? set[key] : defaultValue;
        }
    }
}
