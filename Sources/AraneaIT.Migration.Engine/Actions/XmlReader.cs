// <copyright file="XmlReader.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Actions
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// XML reader provider
    /// </summary>
    [Reader("xml")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class XmlReader : Reader
    {
        /// <summary>
        /// The doc
        /// </summary>
        private XDocument doc;

        /// <summary>
        /// The iterator
        /// </summary>
        private IEnumerator<XElement> iterator;

        /// <summary>
        /// Reads the specified working entity.
        /// </summary>
        /// <param name="workingEntity">The working entity.</param>
        /// <returns>
        /// Populated entity object
        /// </returns>
        public override Entity Read(Entity workingEntity)
        {
            var currentNode = iterator.Current;
            this.Logger.Info("XmlReader: Started loading file");

            foreach (var definition in this.EntityDefinition.PropertiesDefinition)
            {
                workingEntity.AddProperty(
                    definition.Name,
                    definition.Type,
                    currentNode.Descendants(XName.Get(definition.SourceName)).First().Value);
            }

            this.Logger.Info("XmlReader: Finished loading file");
            this.PerformedReading = iterator.MoveNext();

            return workingEntity;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            this.doc = null;
        }

        /// <summary>
        /// Additional the configure.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        protected override void AdditionalConfigure(IDictionary<string, string> parametersSet)
        {
            this.doc = XDocument.Load(File.OpenRead(parametersSet["source-location"]));

            var entityName = parametersSet["entity-name"];
            this.iterator = this.doc.Descendants(XName.Get(entityName)).GetEnumerator();
        }
    }
}