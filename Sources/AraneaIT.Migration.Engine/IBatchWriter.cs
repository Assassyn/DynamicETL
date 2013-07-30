// <copyright file="IBatchWriter.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using System.Collections.Generic;

    /// <summary>
    /// Allows to save a batch of entities to make the process quicker
    /// </summary>
    public interface IBatchWriter : IAction
    {
        /// <summary>
        /// Saves the entites.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void SaveEntites(IEnumerable<Entity> entities);
    }
}