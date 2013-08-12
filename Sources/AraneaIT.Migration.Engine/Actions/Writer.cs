// <copyright file="Writer.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Actions
{
    using NLog;
    using System.Collections.Generic;

    /// <summary>
    /// Generic writer
    /// </summary>
    public abstract class Writer : IWriter
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

            this.Configure(parametersSet);
        }

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="currentEntity">The current entity.</param>
        public abstract void SaveEntity(Entity currentEntity);

        /// <summary>
        /// Saves the batch.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void SaveBatch(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                SaveEntity(entity);
            }
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
        protected virtual void Configure(IDictionary<string, string> parametersSet)
        {
        }

        /// <summary>
        /// Sets the conditions.
        /// </summary>
        /// <param name="readConditions">The read conditions.</param>
        public void SetConditions(IEnumerable<Condition> readConditions)
        {
        }
    }
}