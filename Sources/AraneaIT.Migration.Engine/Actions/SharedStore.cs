// <copyright file="SharedStore.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Actions
{
    using System.Collections.Generic;

    public static class SharedStore
    {
        private static IDictionary<string, object> store;

        static SharedStore()
        {
            SharedStore.store = new Dictionary<string, object>();
        }

        public static void Add(string key, object value)
        {
            SharedStore.store[key] = value;
        }
        public static object Get(string key)
        {
            if (SharedStore.store.ContainsKey(key))
            {
                return SharedStore.store[key];
            }
            else
            {
                return null;
            }
        }
    }
}
