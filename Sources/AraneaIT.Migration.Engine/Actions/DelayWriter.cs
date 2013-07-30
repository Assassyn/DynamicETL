// <copyright file="DelayWriter.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Actions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text;
    using System.Threading;

    [Writer("DelayWriter")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class DelayWriter : Writer
    {
        /// <summary>
        /// The sleep for
        /// </summary>
        private int sleepFor;

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="currentEntity">The current entity.</param>
        public override void SaveEntity(Entity currentEntity)
        {
            Thread.Sleep(this.sleepFor);
        }

        /// <summary>
        /// Additional the configuration.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        protected override void Configure(IDictionary<string, string> parametersSet)
        {
            this.sleepFor = parametersSet.GetValueOrDefault("sleepFor", "1000").ToInt32();
        }
    }
}