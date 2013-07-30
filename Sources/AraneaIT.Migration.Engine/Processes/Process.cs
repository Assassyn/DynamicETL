// <copyright file="Process.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Proceses
{
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Base class for all process handlers
    /// </summary>
    [DebuggerDisplay("Process:{Name},RunAfter:{RunAfter}")]
    public abstract class Process : IDisposable
    {
        /// <summary>
        /// Initializes static members of the <see cref="Process" /> class.
        /// </summary>
        public Process()
        {
            this.Logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Gets or sets the reader collection.
        /// </summary>
        /// <value>
        /// The reader collection.
        /// </value>
        public IEnumerable<IReader> ReaderCollection { get; set; }

        /// <summary>
        /// Gets or sets the converter collection.
        /// </summary>
        /// <value>
        /// The converter collection.
        /// </value>
        public IEnumerable<IConverter> ConverterCollection { get; set; }

        /// <summary>
        /// Gets or sets the writer collection.
        /// </summary>
        /// <value>
        /// The writer collection.
        /// </value>
        public IEnumerable<IWriter> WriterCollection { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the run after.
        /// </summary>
        /// <value>
        /// The run after.
        /// </value>
        public string RunAfter { get; set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected Logger Logger { get; private set; }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (this.ReaderCollection != null)
            {
                foreach (var reader in this.ReaderCollection)
                {
                    reader.Dispose();
                }
            }

            if (this.ConverterCollection != null)
            {
                foreach (var converter in this.ConverterCollection)
                {
                    converter.Dispose();
                }
            }

            if (this.WriterCollection != null)
            {
                foreach (var writer in this.WriterCollection)
                {
                    writer.Dispose();
                }
            }
        }

        /// <summary>
        /// Configures the specified configuration settings.
        /// </summary>
        /// <param name="configurationSet">The configuration settings.</param>
        public void Configure(IDictionary<string, string> configurationSet)
        {
            this.RunAfter = GetValueOrDefault(configurationSet, "run-after", string.Empty);
            this.Name = GetValueOrDefault(configurationSet, "name", string.Empty);

            AdditionalConfiguration(configurationSet);
        }

        /// <summary>
        /// Additional options for configuration.
        /// </summary>
        /// <param name="configurationSettings">The configuration settings.</param>
        protected virtual void AdditionalConfiguration(IDictionary<string, string> configurationSet)
        {
        }

        /// <summary>
        /// Gets the attribute or default.
        /// </summary>
        /// <param name="processNode">The process node.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Getting default value of the attribute</returns>
        protected string GetValueOrDefault(IDictionary<string, string> configurationSet, string name, string defaultValue)
        {
            return configurationSet.ContainsKey(name) ? configurationSet[name] : defaultValue;

            //var attribute = processNode.Attribute(XName.Get(name));

            //return attribute != null ? attribute.Value : defaultValue;
        }
    }
}