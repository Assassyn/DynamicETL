// <copyright file="AppSettingsReader.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Actions
{
    using System.ComponentModel.Composition;
    using System.Configuration;

    /// <summary>
    /// Reading application settings
    /// </summary>
    [Reader("appSettings")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class AppSettingsReader : Reader
    {
        /// <summary>
        /// The performed read
        /// </summary>
        private bool performedRead;

        /// <summary>
        /// The never stop reading
        /// </summary>
        private bool neverStopReading;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsReader"/> class.
        /// </summary>
        public AppSettingsReader()
        {
            this.performedRead = false;
        }

        /// <summary>
        /// Reads the specified working entity.
        /// </summary>
        /// <param name="workingEntity">The working entity.</param>
        /// <returns>
        /// Populated entity object
        /// </returns>
        public override Entity Read(Entity workingEntity)
        {
            if (neverStopReading)
            {
                this.PerformedReading = true;
            }
            else
            {
                this.PerformedReading = !this.performedRead;
                this.performedRead = true;
            }

            foreach (var definition in this.EntityDefinition.PropertiesDefinition)
            {
                var keyName = definition.SourceName;
                var value = ConfigurationManager.AppSettings[keyName];
                workingEntity.AddProperty(
                   definition.Name,
                   definition.Type,
                   value);

                this.Logger.Trace("AppSettingsReader: value for key [{0}] is {1}", keyName, value);
            }

            return workingEntity;
        }

        /// <summary>
        /// Additional the configure.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        protected override void AdditionalConfigure(System.Collections.Generic.IDictionary<string, string> parametersSet)
        {
            this.neverStopReading = (parametersSet.ContainsKey("never-stop-reading") ? parametersSet["never-stop-reading"] : "false").ToBoolean();
        }
    }
}