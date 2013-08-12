// <copyright file="ProcessRunner.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Proceses
{
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Maintain all starting process for declared processes
    /// </summary>
    public sealed class ProcessRunner : IDisposable
    {
        /// <summary>
        /// Occurs when [exeuting finished].
        /// </summary>
        public event EventHandler ExeutingFinished;

        /// <summary>
        /// The _processes
        /// </summary>
        private IEnumerable<Process> processes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessRunner" /> class.
        /// </summary>
        /// <param name="processes">The processes.</param>
        public ProcessRunner(IEnumerable<Process> processes)
        {
            this.processes = processes;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public void Execute()
        {
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            var processesToRun = this.processes.Where(x => string.IsNullOrWhiteSpace(x.RunAfter))
                .ToDictionary(x => x.Name, x => new Task(() => x.Execute(), TaskCreationOptions.LongRunning));
            var restProcesses = this.processes.Where(x => !string.IsNullOrWhiteSpace(x.RunAfter));
            var allProcesses = new Dictionary<string, Task>(processesToRun);

            foreach (var process in restProcesses)
            {
                var previosTasksNames = process.RunAfter + ";";
                var previousTaks = allProcesses.Where(x => previosTasksNames.Contains(x.Key + ";")).Select(x => x.Value).ToArray();

                allProcesses[process.Name] = Task.Factory.ContinueWhenAll(previousTaks, completedTask => process.Execute());
            }

            foreach (var task in processesToRun)
            {
                task.Value.Start(TaskScheduler.Current);
            }

            Task.WaitAll(allProcesses.Select(x => x.Value).ToArray());

            if (this.ExeutingFinished != null)
            {
                this.ExeutingFinished(this, EventArgs.Empty);
            }

            LogManager.GetCurrentClassLogger().Info("Finished processes scheduling");
        }

        void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            LogManager.GetCurrentClassLogger().Error("Task have been canceled");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}