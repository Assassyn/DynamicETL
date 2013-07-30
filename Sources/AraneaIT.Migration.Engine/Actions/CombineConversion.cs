// <copyright file="CombineConversion.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Actions
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Combime two or more properties into another
    /// </summary>
    [Converter("Combine")]
    public sealed class CombineConversion : Converter
    {
        private string combineInto;

        /// <summary>
        /// Performs the specified current entity.
        /// </summary>
        /// <param name="currentEntity">The current entity.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Entity Perform(Entity entity)
        {
            var resultBuilder = new StringBuilder();

            foreach (var propertyDefinition in this.EntityDefinition.PropertiesDefinition)
            {
                resultBuilder.Append(entity[propertyDefinition.SourceName]);
                resultBuilder.Append(" ");
            }

            entity.AddProperty(this.combineInto, resultBuilder.ToString());

            return entity;
        }

        /// <summary>
        /// Additional the configuration.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        protected override void AdditionalConfiguration(IDictionary<string, string> parametersSet)
        {
            this.combineInto = parametersSet["combine-into"];
        }
    }
}