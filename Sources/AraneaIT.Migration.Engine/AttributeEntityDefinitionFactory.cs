// <copyright file="XMLEntityFactory.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// EntityDefinition factory based on object attributes.
    /// </summary>
    public static class AttributeEntityDefinitionFactory
    {
        /// <summary>
        /// Creates the entity D efinition.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public static IEnumerable<PropertyDescriptor> CreateEntityDefinition(string typeName, bool ignoreSourceName = false)
        {
            var type = Type.GetType(typeName);

            return AttributeEntityDefinitionFactory.CreateEntityDefinition(type, ignoreSourceName);
        }

        /// <summary>
        /// Creates the entity D efinition.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        public static IEnumerable<PropertyDescriptor> CreateEntityDefinition(Type entityType, bool ignoreSourceName = false)
        {
            var propertiesDefinitions = new HashSet<PropertyDescriptor>();

            foreach (var property in entityType.GetProperties())
            {
                var attribute = property.GetCustomAttributes(typeof(PropertyAttribute), true).FirstOrDefault() as PropertyAttribute;

                if (attribute != null)
                {
                    var name = property.Name;
                    var type = property.PropertyType.FullName;

                    var sourceName = property.Name;
                    var required = attribute.Required;

                    if (!ignoreSourceName)
                    {
                        if (!string.IsNullOrWhiteSpace(attribute.SourceName))
                        {
                            sourceName = attribute.SourceName;
                        }
                    }

                    propertiesDefinitions.Add(new PropertyDescriptor(
                        name: name,
                        type: type,
                        sourceName: sourceName,
                        required: required));
                }
            }

            return propertiesDefinitions;
        }
    }
}