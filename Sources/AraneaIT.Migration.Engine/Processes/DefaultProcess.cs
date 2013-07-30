// <copyright file="DefaultProcess.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Proceses
{
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Class handling the defined process.
    /// </summary>
    [Process("Default")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class DefaultProcess : Process
    {
        /// <summary>
        /// The tasks
        /// </summary>
        private HashSet<Task> tasks;

        /// <summary>
        /// The continue process
        /// </summary>
        private bool continueProcess;

        /// <summary>
        /// The timer
        /// </summary>
        private Stopwatch timer;

        /// <summary>
        /// The counter
        /// </summary>
        private int counter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultProcess" /> class.
        /// </summary>
        public DefaultProcess()
        {
            this.timer = new Stopwatch();
            this.continueProcess = true;
            this.tasks = new HashSet<Task>();
            this.counter = 0;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override void Execute()
        {
            try
            {
                var cancelationToken = new System.Threading.CancellationToken();
                cancelationToken.Register(
                    () => LogManager.GetCurrentClassLogger().Error("Task have been canceled"), true);

                this.Logger.Info("Process[{0}]: Started", this.Name);

                this.timer.Start();

                while (this.continueProcess)
                {
                    var currentEntity = new Entity();

                    foreach (var reader in this.ReaderCollection)
                    {
                        currentEntity = reader.Read(currentEntity);

                        if (!reader.PerformedReading)
                        {
                            this.continueProcess = false;
                            break;
                        }
                    }

                    if (this.continueProcess)
                    {
                        this.Logger.Debug("Proces[{0}]: Entity no: {1} processed", this.Name, this.counter);
                        this.counter++;

                        this.tasks.Add(Task.Factory.StartNew(
                            action: () => this.PerformAfterReadingActions(currentEntity),
                            cancellationToken: cancelationToken,
                            creationOptions: TaskCreationOptions.LongRunning & TaskCreationOptions.PreferFairness,
                            scheduler: TaskScheduler.Current));
                    }
                }

                Task.WaitAll(this.tasks.ToArray());

                this.Logger.Trace("Proces[{0}]: Time taken: {1}", this.Name, this.timer.Elapsed);
                this.Logger.Debug("Proces[{0}]: Finished with {1} entities", this.Name, this.counter);
                this.Logger.Info("Process[{0}]: Finished", this.Name);
            }
            catch (Exception exp)
            {
                this.Logger.Fatal(exp);
            }
        }

        /// <summary>
        /// Tasks the action.
        /// </summary>
        /// <param name="taskState">State of the task.</param>
        private void TaskAction(object taskState)
        {
            try
            {
                var entity = taskState as Entity;
                this.PerformAfterReadingActions(entity);
            }
            catch (Exception exp)
            {
                this.Logger.Fatal(exp);
            }
        }

        /// <summary>
        /// Performs the after reading actions.
        /// </summary>
        /// <param name="currentEntity">The current entity.</param>
        private void PerformAfterReadingActions(Entity currentEntity)
        {
            try
            {
                this.Logger.Trace("Process: Processing entities");

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
            }
            catch (Exception exp)
            {
                this.Logger.Fatal(exp);
            }
        }
    }
}