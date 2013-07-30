// <copyright file="Configurator.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Configuration
{
    using AraneaIT.Migration.Engine.Proceses;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;

    public abstract class Configurator : IConfigurator
    {
        /// <summary>
        /// The container
        /// </summary>
        private CompositionContainer container;

        public bool IsConfigured { get; protected set; }

        public ProcessRunner GetProcessRunner()
        {
            this.container = new CompositionContainer(new DirectoryCatalog("."));
            this.container.Compose(new CompositionBatch());

            var processes = this.GetProcesses(this.container);
            return new ProcessRunner(processes);
        }

        public virtual void Configure(IDictionary<string, string> configurationSet)
        {
        }

        public void Dispose()
        {
            if (this.container != null)
            {
                this.container.Dispose();
            }
        }

        protected abstract IEnumerable<Process> GetProcesses(CompositionContainer mefContainer);
    }
}