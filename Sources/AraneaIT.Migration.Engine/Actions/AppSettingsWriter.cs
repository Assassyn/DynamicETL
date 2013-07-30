// <copyright file="AppSettingsWriter.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Actions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using NLog;
    using System.Configuration;
    using System.Reflection;
    using System.ComponentModel.Composition;

    /// <summary>
    /// Writer for the app settings section.
    /// </summary>
    [Writer("appSettings")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class AppSettingsWriter : Writer
    {
        /// <summary>
        /// The override value
        /// </summary>
        private bool overrideValue;

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="currentEntity">The current entity.</param>
        public override void SaveEntity(Entity currentEntity)
        {
            foreach (var propertyDefinition in this.EntityDefinition.PropertiesDefinition)
            {
                var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);
                var appSettingsSection = config.GetSection("appSettings") as AppSettingsSection;
                if (this.overrideValue || !appSettingsSection.Settings.AllKeys.Contains(propertyDefinition.Name))
                {
                    appSettingsSection.Settings.Remove(propertyDefinition.Name);
                    var value = currentEntity[propertyDefinition.SourceName].ToString();
                    appSettingsSection.Settings.Add(propertyDefinition.Name, value);
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");

                    this.Logger.Trace("AppSettingsWriter: value for key [{0}] is {1}", propertyDefinition.Name, value);
                }
            }
        }

        /// <summary>
        /// Additional the configuration.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        protected override void Configure(IDictionary<string, string> parametersSet)
        {
            this.overrideValue = parametersSet.GetValueOrDefault("override", "false").ToBoolean();
        }
    }
}