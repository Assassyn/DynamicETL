// <copyright file="EntityDefinition.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>
namespace AraneaIT.Migration.Engine
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Basic interface to create custom definitions for every configuration avaialble
    /// </summary>
    public interface IEntityDefinition
    {
        void AddProperty(PropertyDescriptor property);

        /// <summary>
        /// Gets the properties definition.
        /// </summary>
        /// <value>
        /// The properties definition.
        /// </value>
        ReadOnlyCollection<PropertyDescriptor> PropertiesDefinition { get; }
    }
}
