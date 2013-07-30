// <copyright file="AppSettingsWriter.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Actions
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    /// <summary>
    /// Writer for the app settings section.
    /// </summary>
    [Writer("SharedStore")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class SharedStoreWriter : Writer
    {
        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="currentEntity">The current entity.</param>
        public override void SaveEntity(Entity currentEntity)
        {
            foreach (var definition in this.EntityDefinition.PropertiesDefinition)
            {
                SharedStore.Add(definition.Name, currentEntity[definition.SourceName]);
            }
        }

        /// <summary>
        /// Additional the configuration.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        protected override void Configure(IDictionary<string, string> parametersSet)
        {
        }
    }
}