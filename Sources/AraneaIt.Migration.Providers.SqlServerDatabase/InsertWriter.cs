// <copyright file="InserWriter.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Providers.SqlServerDatabase
{
    using AraneaIT.Migration.Engine;
    using AraneaIT.Migration.Engine.Actions;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    /// <summary>
    /// Creating insert into Microsoft SQL database
    /// </summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Writer("MsSQL")]
    internal sealed class InsertWriter : Writer
    {
        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="currentEntity">The current entity.</param>
        public override void SaveEntity(Entity currentEntity)
        {
        }

        /// <summary>
        /// Saves the batch.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public override void SaveBatch(IEnumerable<Entity> entities)
        {
            base.SaveBatch(entities);
        }
    }
}