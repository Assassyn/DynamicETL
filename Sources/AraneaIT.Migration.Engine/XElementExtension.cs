// <copyright file="XElementExtension.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using AraneaIT.Migration.Engine.Configuration;
    using System.Collections.Generic;
    using System.Xml.Linq;

    /// <summary>
    /// Extension which allows to get default value when attribute have not bee found
    /// </summary>
    public static class XElementExtension
    {
        /// <summary>
        /// Gets the value or default.
        /// </summary>
        /// <param name="processNode">The process node.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// attribute value or default value provided
        /// </returns>
        public static string GetAttributeValueOrDefault(this XElement processNode, string name, string defaultValue)
        {
            var attribute = processNode.Attribute(XName.Get(name));

            var returnValue = defaultValue;

            if (attribute != null)
            {
                returnValue = attribute.Value;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the attribute or default.
        /// </summary>
        /// <param name="processNode">The process node.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns>
        /// converted to boolean value or provided default
        /// </returns>
        public static bool GetAttributeValueOrDefault(this XElement processNode, string name, bool defaultValue)
        {
            var attribute = processNode.Attribute(XName.Get(name));

            var returnValue = defaultValue;

            if (attribute != null)
            {
                bool.TryParse(attribute.Value, out returnValue);
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the entity definition properties.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static IList<PropertyDescriptor> GetEntityDefinitionProperties(this XElement node)
        {
            var properties = new List<PropertyDescriptor>();

            var entityNode = node.Element(XName.Get("entity"));
            if (entityNode != null)
            {
                var entityType = entityNode.Attribute(XName.Get("type"));

                if (entityType != null)
                {
                    var ignoreSourceNameAttrubute = entityNode.Attribute("ignore-source-name");
                    var ignoreSourceName = false;

                    if (ignoreSourceNameAttrubute != null)
                    {
                        ignoreSourceName = ignoreSourceNameAttrubute.Value.ToBoolean();
                    }

                    properties.AddRange(AttributeEntityDefinitionFactory.CreateEntityDefinition(entityType.Value, ignoreSourceName));
                }

                properties.AddRange(XMLEntityDefinitionFactory.CreateEntityDefinition(entityNode));
            }

            return properties;
        }

        /// <summary>
        /// Gets the entity definition.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static IEntityDefinition GetEntityDefinition(this XElement node)
        {
            return new EntityDefinition(node.GetEntityDefinitionProperties());
        }

        /// <summary>
        /// Gets the join on attribute.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns></returns>
        public static string GetJoinOnAttribute(this XElement node, string attributeName)
        {
            var onNode = node.Element("on");

            return onNode.GetAttributeValueOrDefault(attributeName, string.Empty);

        }
    }
}