// <copyright file="IConverter.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using System.Collections.Generic;

    /// <summary>
    /// Generic converter
    /// </summary>
    public interface IConverter : IAction
    {
        /// <summary>
        /// Performs the specified current entity.
        /// </summary>
        /// <param name="currentEntity">The current entity.</param>
        /// <returns>Updated entity</returns>
        Entity Perform(Entity currentEntity);

        /// <summary>
        /// Perfoms for batch.
        /// </summary>
        /// <param name="IEnumerable`1">The I enumerable`1.</param>
        /// <returns>Batch of updated entities</returns>
        IEnumerable<Entity> PerfomForBatch(IEnumerable<Entity> entities);
    }
}