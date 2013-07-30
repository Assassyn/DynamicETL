// <copyright file="Entity.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;

    /// <summary>
    /// Entity object
    /// </summary>
    [DebuggerDisplay("Entity|PropertiesCount:{PropertiesDictionary.Count}")]
    public sealed class Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity" /> class.
        /// </summary>
        public Entity()
        {
            this.PropertiesDictionary = new SortedDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets or sets the properties dictionary.
        /// </summary>
        /// <value>
        /// The properties dictionary.
        /// </value>
        public IDictionary<string, object> PropertiesDictionary { get; set; }

        /// <summary>
        /// Default getter for property
        /// </summary>
        /// <value>
        /// The <see cref="System.Object" />.
        /// </value>
        /// <param name="name">A name of property</param>
        /// <returns>
        /// Value of property or empty string
        /// </returns>
        public object this[string name]
        {
            get
            {
                if (this.PropertiesDictionary.ContainsKey(name))
                {
                    return this.PropertiesDictionary[name];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Adding a property with converted value
        /// </summary>
        /// <param name="name">name of property</param>
        /// <param name="type">a desired type default value is System.String</param>
        /// <param name="value">a value of parameter</param>
        public void AddProperty(string name, string type, object value = null)
        {
            this.PropertiesDictionary[name] = this.ConvertType(value, type);
        }

        /// <summary>
        /// Values as.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>Value converted to requested type</returns>
        public TType ValueAs<TType>(string name)
        {
            var result = this[name] ?? default(TType);
            return (TType)result;
        }

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>default value</returns>
        private static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Tries the execute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="action">The action.</param>
        /// <param name="result">The result.</param>
        /// <returns>true when no error</returns>
        private static bool TryExecute(Type type, Func<object> action, out object result)
        {
            var executed = false;
            try
            {
                result = action();
                executed = true;
            }
            catch
            {
                result = Entity.GetDefault(type);
            }

            return executed;
        }

        /// <summary>
        /// Converts the type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>converted value</returns>
        private object ConvertType(object value, string typeName)
        {
            var type = Type.GetType(typeName, false, true) ?? typeof(string);

            object result;

            if (value == null)
            {
                result = Entity.GetDefault(type);
            }
            else if (value.GetType() == type)
            {
                result = value;
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(type);
                var valueOrEmptyString = (value ?? string.Empty).ToString();

                if (!Entity.TryExecute(type, () => converter.ConvertFrom(valueOrEmptyString), out result))
                {
                    Entity.TryExecute(type, () => converter.ConvertFromInvariantString(valueOrEmptyString), out result);
                }
            }

            return result;
        }
    }
}