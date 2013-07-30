// <copyright file="IConfigurator.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine
{
    using AraneaIT.Migration.Engine.Proceses;
    using System;

    /// <summary>
    /// Main interface to allow to create custom configuration for the process runner
    /// </summary>
    public interface IConfigurator : IDisposable
    {
        bool IsConfigured { get; }

        /// <summary>
        /// Gets the process runner.
        /// </summary>
        /// <returns>Process runner to start to processing</returns>
        ProcessRunner GetProcessRunner();
    }
}