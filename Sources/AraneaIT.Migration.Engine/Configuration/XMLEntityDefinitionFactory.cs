// <copyright file="XMLEntityDefinitionFactory.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Creatiung properties collection based on the XML configuration node.
    /// </summary>
    public static class XMLEntityDefinitionFactory
    {
        /// <summary>
        /// Creates the entity definition.
        /// </summary>
        /// <param name="entityNode">The entity node.</param>
        /// <returns></returns>
        public static IEnumerable<PropertyDescriptor> CreateEntityDefinition(XElement entityNode)
        {
            return entityNode
                    .Elements(XName.Get("property"))
                    .Select(x => CreateDescriptor(x));
        }

        /// <summary>
        /// Creates the descriptor.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        private static PropertyDescriptor CreateDescriptor(XElement x)
        {
            var name = x.Attribute("name").Value;

            var type = "System.String";
            var typeNode = x.Attribute("type");
            if (typeNode != null)
            {
                type = typeNode.Value;
            }

            string sourceName = null;
            var sourceNameNode = x.Attribute("source-name");
            if (sourceNameNode != null)
            {
                sourceName = sourceNameNode.Value;
            }

            var required = true;
            var requiredNode = x.Attribute("required");
            if (requiredNode != null)
            {
                required = bool.Parse(requiredNode.Value);
            }

            return new PropertyDescriptor(name, type, sourceName, required);
        }
    }
}