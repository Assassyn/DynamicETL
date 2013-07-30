// <copyright file="AppSettingsReader.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Actions
{
    using System.ComponentModel.Composition;

    /// <summary>
    /// Reading application settings
    /// </summary>
    [Reader("SharedStore")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class SharedStoreReader : Reader
    {
        /// <summary>
        /// Reads the specified working entity.
        /// </summary>
        /// <param name="workingEntity">The working entity.</param>
        /// <returns>
        /// Populated entity object
        /// </returns>
        public override Entity Read(Entity workingEntity)
        {
            this.PerformedReading = true;

            foreach (var definition in this.EntityDefinition.PropertiesDefinition)
            {
                workingEntity.AddProperty(
                   definition.Name,
                   definition.Type,
                   SharedStore.Get(definition.Name));
            }

            return workingEntity;
        }

        /// <summary>
        /// Additional the configure.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        protected override void AdditionalConfigure(System.Collections.Generic.IDictionary<string, string> parametersSet)
        {
        }
    }
}