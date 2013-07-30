// <copyright file="EntityDefinition.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Simple implementation of the Entitydefinition.
    /// </summary>
    public sealed class EntityDefinition : IEntityDefinition
    {
        private List<PropertyDescriptor> properties;
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDefinition"/> class.
        /// </summary>
        /// <param name="propertiesDefinitions">The properties definitions.</param>
        public EntityDefinition(IEnumerable<PropertyDescriptor> propertiesDefinitions)
        {
            this.properties = propertiesDefinitions.ToList();
        }

        /// <summary>
        /// Adds the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void AddProperty(PropertyDescriptor property)
        {
            this.properties.Add(property);
        }

        /// <summary>
        /// Gets the properties definition.
        /// </summary>
        /// <value>
        /// The properties definition.
        /// </value>
        public ReadOnlyCollection<PropertyDescriptor> PropertiesDefinition
        {
            get { return this.properties.AsReadOnly(); }
        }
    }
}