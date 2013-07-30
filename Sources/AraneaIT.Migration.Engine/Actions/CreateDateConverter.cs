// <copyright file="CreateDateConverter.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Actions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    /// <summary>
    /// Allows to create date without any data source
    /// </summary>
    [Converter("create-date")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class CreateDateConverter : Converter
    {
        /// <summary>
        /// The date
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Performs the specified current entity.
        /// </summary>
        /// <param name="currentEntity">The current entity.</param>
        /// <returns>Entity with added date property</returns>
        public override Entity Perform(Entity currentEntity)
        {
            foreach (var definition in this.EntityDefinition.PropertiesDefinition)
            {
                currentEntity.AddProperty(
                    definition.Name,
                    "System.DateTime",
                    this.date);
            }

            return currentEntity;
        }

        /// <summary>
        /// Additional the configuration.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        protected override void AdditionalConfiguration(IDictionary<string, string> parametersSet)
        {
            var dateFormat = parametersSet.ContainsKey("date") ? parametersSet["date"] : "today";

            if (dateFormat.Equals("today", StringComparison.OrdinalIgnoreCase))
            {
                this.date = DateTime.Now;
            }
            else if (dateFormat.Equals("yesterday", StringComparison.OrdinalIgnoreCase))
            {
                this.date = DateTime.Now.AddDays(-1);
            }
            else if (dateFormat.Equals("tomorrow", StringComparison.OrdinalIgnoreCase))
            {
                this.date = DateTime.Now.AddDays(1);
            }
            else
            {
                DateTime.TryParse(dateFormat, out this.date);
            }
        }
    }
}
