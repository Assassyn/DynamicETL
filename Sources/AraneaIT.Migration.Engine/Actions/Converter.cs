// <copyright file="Writer.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Actions
{
    using NLog;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Generic converter
    /// </summary>
    public abstract class Converter : IConverter
    {
        /// <summary>
        /// Gets or sets the entity definition.
        /// </summary>
        /// <value>
        /// The entity definition.
        /// </value>
        protected IEntityDefinition EntityDefinition { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected Logger Logger { get; set; }

        /// <summary>
        /// Configures the specified parameters set.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        /// <param name="entityDefinition">The entity definition.</param>
        public void Configure(IDictionary<string, string> parametersSet, IEntityDefinition entityDefinition)
        {
            this.Logger = LogManager.GetCurrentClassLogger();
            this.EntityDefinition = entityDefinition;

            this.AdditionalConfiguration(parametersSet);
        }

        /// <summary>
        /// Performs the specified current entity.
        /// </summary>
        /// <param name="currentEntity">The current entity.</param>
        /// <returns></returns>
        public abstract Entity Perform(Entity currentEntity);

        /// <summary>
        /// Perfoms for batch.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns>
        /// Batch of updated entities
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<Entity> PerfomForBatch(IEnumerable<Entity> entities)
        {
            var resultCollectino = new List<Entity>(entities.Count());

            foreach (var entity in entities)
            {
                resultCollectino.Add(this.Perform(entity));
            }

            return resultCollectino;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
        }


        /// <summary>
        /// Additional the configuration.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        protected virtual void AdditionalConfiguration(IDictionary<string, string> parametersSet)
        {
        }

        /// <summary>
        /// Sets the conditions.
        /// </summary>
        /// <param name="readConditions">The read conditions.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SetConditions(IEnumerable<Condition> readConditions)
        {
        }
    }
}