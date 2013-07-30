// <copyright file="BatchReadProcess.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Proceses
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Process without read step
    /// </summary>
    [Process("Save")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class SaveProcess : Process
    {
        /// <summary>
        /// The batch size
        /// </summary>
        private int batchSize;

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override void Execute()
        {
            var task = Task.Factory.StartNew(() =>
                {
                    this.Logger.Trace("Process: Processing entities");

                    var currentEntity = new Entity();

                    foreach (var converter in this.ConverterCollection)
                    {
                        currentEntity = converter.Perform(currentEntity);
                    }

                    this.Logger.Trace("Process: Saving entities");

                    foreach (var writer in this.WriterCollection)
                    {
                        writer.SaveEntity(currentEntity);
                    }

                    this.Logger.Trace("Process: Finished processing entity");
                });

            Task.WaitAll(new Task[] { task });
        }

        /// <summary>
        /// Additional options for configuration.
        /// </summary>
        /// <param name="configurationSet"></param>
        protected override void AdditionalConfiguration(IDictionary<string, string> configurationSet)
        {
            this.batchSize = int.Parse(GetValueOrDefault(configurationSet, "batch-size", "1"));
        }
    }
}