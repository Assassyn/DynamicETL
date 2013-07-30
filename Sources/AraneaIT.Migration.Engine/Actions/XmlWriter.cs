// <copyright file="XmlWriter.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Actions
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    /// <summary>
    ///
    /// </summary>
    [Writer("xml")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class XmlWriter : Writer
    {
        /// <summary>
        /// The file path
        /// </summary>
        private string filePath;

        /// <summary>
        /// The entity name
        /// </summary>
        private string entityName;

        /// <summary>
        /// The writer
        /// </summary>
        private System.Xml.XmlWriter writer;

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="currentEntity">The current entity.</param>
        public override void SaveEntity(Entity currentEntity)
        {
            this.Logger.Trace("Started save process");

            this.writer.WriteStartElement(entityName);
            foreach (var property in this.EntityDefinition.PropertiesDefinition)
            {
                this.writer.WriteStartElement(property.Name);
                this.writer.WriteValue(currentEntity.ValueAs<string>(property.SourceName));
                this.writer.WriteEndElement();
            }
            this.writer.WriteEndElement();
            this.writer.Flush();

            this.Logger.Trace("Finished save process");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (this.writer != null)
            {
                this.writer.WriteEndElement();
                this.writer.Flush();
                this.writer.Close();
            }
        }

        /// <summary>
        /// Additional the configuration.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        protected override void Configure(IDictionary<string, string> parametersSet)
        {
            this.filePath = parametersSet["file-name"];
            this.entityName = parametersSet["entity-name"];

            this.writer = System.Xml.XmlWriter.Create(parametersSet["file-name"]);
            this.writer.WriteStartDocument(true);
            this.writer.WriteStartElement(parametersSet["root-name"]);
        }
    }
}