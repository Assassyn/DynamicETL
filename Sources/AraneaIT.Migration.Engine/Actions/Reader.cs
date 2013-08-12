// <copyright file="Reader.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>
namespace AraneaIT.Migration.Engine.Actions
{
    using NLog;
    using System.Collections.Generic;

    /// <summary>
    /// Abstract IReader implementation
    /// </summary>
    public abstract class Reader : IReader
    {
        /// <summary>
        /// Gets or sets a value indicating whether [performed reading].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [performed reading]; otherwise, <c>false</c>.
        /// </value>
        public bool PerformedReading { get; protected set; }

        /// <summary>
        /// Gets or sets the entity definition.
        /// </summary>
        /// <value>
        /// The entity definition.
        /// </value>
        protected IEntityDefinition EntityDefinition { get; set; }

        /// <summary>
        /// Gets or sets the condition collection.
        /// </summary>
        /// <value>
        /// The condition collection.
        /// </value>
        protected IEnumerable<Condition> Conditions { get; set; }

        /// <summary>
        /// Gets or sets the joins.
        /// </summary>
        /// <value>
        /// The joins.
        /// </value>
        protected IEnumerable<Join> Joins { get; set; }

        /// <summary>
        /// Gets or sets the size of the batch.
        /// </summary>
        /// <value>
        /// The size of the batch.
        /// </value>
        public int? BatchSize { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected Logger Logger { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Reader"/> class.
        /// </summary>
        public Reader()
        {
            this.Joins = new List<Join>();
        }

        /// <summary>
        /// Configures the specified parameters set.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        /// <param name="entityDefinition">The entity definition.</param>
        public void Configure(IDictionary<string, string> parametersSet, IEntityDefinition entityDefinition)
        {
            this.EntityDefinition = entityDefinition;
            this.Logger = LogManager.GetCurrentClassLogger();

            this.AdditionalConfigure(parametersSet);
        }

        /// <summary>
        /// Sets the read conditions.
        /// </summary>
        /// <param name="conditions">The conditions.</param>
        public void SetConditions(IEnumerable<Condition> conditions)
        {
            this.Conditions = conditions;
        }

        /// <summary>
        /// Adds the join.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="masterKey">The master key.</param>
        /// <param name="entityKey">The entity key.</param>
        /// <param name="dataDefinition">The data definition.</param>
        public void SetJoins(IEnumerable<Join> joins)
        {
            this.Joins = joins;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// Reads the specified working entity.
        /// </summary>
        /// <param name="workingEntity">The working entity.</param>
        /// <returns>Populated entity object</returns>
        public abstract Entity Read(Entity workingEntity);

        /// <summary>
        /// Additional the configure.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        /// <param name="entityDefinition">The entity definition.</param>
        protected virtual void AdditionalConfigure(IDictionary<string, string> parametersSet)
        {
        }
    }
}