// <copyright file="Iwriter.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contract to allow the feedback to any source
    /// </summary>
    public interface IWriter : IAction
    {
        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void SaveEntity(Entity entity);

        /// <summary>
        /// Saves the batch.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void SaveBatch(IEnumerable<Entity> entities);
    }
}
