// <copyright file="Join.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    /// <summary>
    /// Allows to define join inside a reader. For now it is going to work only with data bases.
    /// </summary>
    public sealed class Join
    {
        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        /// <value>
        /// The name of the entity.
        /// </value>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the master key.
        /// </summary>
        /// <value>
        /// The master key.
        /// </value>
        public string MasterKey { get; set; }

        /// <summary>
        /// Gets or sets the master entity.
        /// </summary>
        /// <value>
        /// The master entity.
        /// </value>
        public string MasterEntity {get;set;}

        /// <summary>
        /// Gets or sets the entity key.
        /// </summary>
        /// <value>
        /// The entity key.
        /// </value>
        public string EntityKey { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the entity definition.
        /// </summary>
        /// <value>
        /// The entity definition.
        /// </value>
        public IEntityDefinition EntityDefinition { get; set; }
    }
}