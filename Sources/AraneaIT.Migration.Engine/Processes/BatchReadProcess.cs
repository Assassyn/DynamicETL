// <copyright file="BatchReadProcess.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Proceses
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Class handling the defined process.
    /// </summary>
    [Process("Batch")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class BatchReadProcess : Process
    {
        /// <summary>
        /// The process lock
        /// </summary>
        private static object processLock = new object();

        private const int TaskToRunAtOnceCount = 2;

        /// <summary>
        /// The batch size
        /// </summary>
        private int batchSize;

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

        public BatchReadProcess()
        {
            this.timer = new Stopwatch();
            this.counter = 0;
            this.tasks = new HashSet<Task>();
            this.continueProcess = true;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override void Execute()
        {
            this.timer.Start();

            this.Logger.Info("Batch process \"{0}\" started", this.Name);

            while (this.continueProcess)
            {
                var entities = new List<Entity>(this.batchSize);

                this.Logger.Info("Batch process \"{0}\": reading entities no: {1}", this.Name, counter);
                counter++;

                for (int index = 0; index < this.batchSize; ++index)
                {
                    var entity = new Entity();

                    foreach (var reader in this.ReaderCollection)
                    {
                        entity = reader.Read(entity);

                        if (!reader.PerformedReading)
                        {
                            this.continueProcess = false;
                            break;
                        }
                    }

                    if (this.continueProcess)
                    {
                        entities.Add(entity);
                    }
                    else
                    {
                        break;
                    }
                }

                if (this.continueProcess || entities.Count() > 0)
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        this.Logger.Trace("Process: Processing entities");

                        foreach (var converter in this.ConverterCollection)
                        {
                            entities = converter.PerfomForBatch(entities).ToList();
                        }

                        this.Logger.Trace("Process: Saving entities");

                        foreach (var writer in this.WriterCollection)
                        {
                            writer.SaveBatch(entities);
                        }

                        this.Logger.Trace("Process: Finished processing entity");
                    },
                    TaskCreationOptions.LongRunning & TaskCreationOptions.PreferFairness));

                    if (tasks.Count > BatchReadProcess.TaskToRunAtOnceCount)
                    {
                        lock (BatchReadProcess.processLock)
                        {
                            if (tasks.Count > BatchReadProcess.TaskToRunAtOnceCount)
                            {
                                Task.WaitAll(tasks.ToArray());
                                foreach (var task in tasks)
                                {
                                    task.Dispose();
                                }
                                tasks.Clear();
                            }
                        }
                    }
                }
            }

            Task.WaitAll(tasks.ToArray());

            this.timer.Stop();

            this.Logger.Trace(" Time taken: {0}", timer.Elapsed);
            this.Logger.Info("Batch process {0} finished", this.Name);
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