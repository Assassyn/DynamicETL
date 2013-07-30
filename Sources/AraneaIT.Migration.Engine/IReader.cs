// <copyright file="IReader.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public interface IReader : IAction
    {
        /// <summary>
        /// Gets a value indicating whether [performed reading].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [performed reading]; otherwise, <c>false</c>.
        /// </value>
        bool PerformedReading { get; }

        /// <summary>
        /// Gets or sets the size of the batch.
        /// </summary>
        /// <value>
        /// The size of the batch.
        /// </value>
        int? BatchSize { get; set; }

        /// <summary>
        /// Sets the read conditions.
        /// </summary>
        /// <param name="readConditions">The enumerable.</param>
        void SetReadConditions(IEnumerable<ReadCondition> readConditions);

        /// <summary>
        /// Adds the join.
        /// </summary>
        /// <param name="joins">The joins.</param>
        void SetJoins(IEnumerable<Join> joins);

        /// <summary>
        /// Reads the specified working entity.
        /// </summary>
        /// <param name="workingEntity">The working entity.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns></returns>
        Entity Read(Entity workingEntity);
    }
}