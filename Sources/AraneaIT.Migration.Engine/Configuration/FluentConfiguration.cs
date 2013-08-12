namespace AraneaIT.Migration.Engine.Configuration
{
    using AraneaIT.Migration.Engine.Proceses;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using System.Text;

    public sealed class FluentConfiguration : Configurator
    {
        private List<ProcessFluentConfiuguration> processesConfiguration;

        public static ProcessFluentConfiuguration AddProcess(string name, bool staticVariable = true)
        {
            return new FluentConfiguration().AddProcess(name);
        }

        private FluentConfiguration()
        {
            this.processesConfiguration = new List<ProcessFluentConfiuguration>();
        }

        public ProcessFluentConfiuguration AddProcess(string name)
        {
            var process = new ProcessFluentConfiuguration(this, name);
            this.processesConfiguration.Add(process);
            return process;
        }

        protected override IEnumerable<Process> GetProcesses(CompositionContainer mefContainer)
        {
            return this.processesConfiguration.Select(x => x.ToProcess(mefContainer));
        }

        public sealed class ProcessFluentConfiuguration
        {
            private FluentConfiguration parent;
            private List<IReader> readers;
            private List<IWriter> writers;
            private List<IConverter> converters;

            internal ProcessFluentConfiuguration(FluentConfiguration parent, string name)
            {
                this.parent = parent;
                this.Name = name;
                this.readers = new List<IReader>();
                this.writers = new List<IWriter>();
                this.converters = new List<IConverter>();
            }

            public ProcessFluentConfiuguration AddProcess(string name)
            {
                return this.parent.AddProcess(name);
            }

            public ProcessFluentConfiuguration Configure(IDictionary<string, string> configuration)
            {
                this.Configuration = configuration;
                return this;
            }

            public ProcessFluentConfiuguration AddReader(IReader reader)
            {
                this.readers.Add(reader);
                return this;
            }
            
            public ProcessFluentConfiuguration AddWriter(IWriter writer)
            {
                this.writers.Add(writer);
                return this;
            }

            public ProcessFluentConfiuguration AddConverter(IConverter converter)
            {
                this.converters.Add(converter);
                return this;
            }

            public string Name { get; private set; }

            public IDictionary<string, string> Configuration { get; set; }

            internal Process ToProcess(CompositionContainer mefContainer)
            {
                var process = mefContainer.GetExportedValueOrDefault<Process>(this.Name);

                process.ReaderCollection = this.readers;
                process.ConverterCollection = this.converters;
                process.WriterCollection = this.writers;
                process.Configure(this.Configuration);

                return process;
            }
        }
    }
}
