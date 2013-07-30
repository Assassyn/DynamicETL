// <copyright file="PropertyDescriptor.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

using System.Diagnostics;
namespace AraneaIT.Migration.Engine
{
    /// <summary>
    /// Describe properties
    /// </summary>
    [DebuggerDisplay("Name:{Name};SourceName:{SourceName};Type:{Type};Required:{Required}")]
    public sealed class PropertyDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDescriptor"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="sourceName">Name of the source.</param>
        /// <param name="required">if set to <c>true</c> [required].</param>
        /// <param name="isKey">if set to <c>true</c> [is key].</param>
        public PropertyDescriptor(string name, string type = "System.String", string sourceName = null, bool required = true, bool isKey = false)
        {
            this.Name = name;
            this.Type = type;
            this.SourceName = sourceName ?? name;
            this.Required = required;
            this.IsKeyProperty = isKey;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the name of the source.
        /// </summary>
        /// <value>
        /// The name of the source.
        /// </value>
        public string SourceName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="PropertyDescriptor"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        public bool Required { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is key property.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is key property; otherwise, <c>false</c>.
        /// </value>
        public bool IsKeyProperty { get; private set; }
    }
}