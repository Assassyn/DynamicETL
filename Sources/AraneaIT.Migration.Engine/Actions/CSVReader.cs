
namespace AraneaIT.Migration.Engine.Actions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Allows to read CSV file
    /// </summary>
    [Reader("csv")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class CSVReader : Reader
    {
        /// <summary>
        /// The CSV split regex
        /// </summary>
        private const string CsvSplitRegex = "(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)";

        /// <summary>
        /// The file
        /// </summary>
        private StreamReader file;

        /// <summary>
        /// Reads the specified working entity.
        /// </summary>
        /// <param name="workingEntity">The working entity.</param>
        /// <returns>
        /// Populated entity object
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Entity Read(Entity workingEntity)
        {
            var line = this.file.ReadLine();
            this.PerformedReading = false;

            if (!string.IsNullOrEmpty(line))
            {
                this.PerformedReading = true;
                var items = Regex.Split(line, CSVReader.CsvSplitRegex);

                foreach (var definition in this.EntityDefinition.PropertiesDefinition)
                {
                    var index = int.Parse(definition.SourceName);

                    workingEntity.AddProperty(
                        definition.Name,
                        definition.Type,
                        items[index].Trim());
                }
            }

            return workingEntity;

            //var currentNode = iterator.Current;
            //this.Logger.Info("XmlReader: Started loading file");

            //foreach (var definition in this.EntityDefinition.PropertiesDefinition)
            //{
            //    workingEntity.AddProperty(
            //        definition.Name,
            //        definition.Type,
            //        currentNode.Descendants(XName.Get(definition.SourceName)).First().Value);
            //}

            //this.Logger.Info("XmlReader: Finished loading file");
            //this.PerformedReading = iterator.MoveNext();

            //return workingEntity;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (this.file != null)
            {
                this.file.Dispose();
            }
        }

        /// <summary>
        /// Additional the configure.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        protected override void AdditionalConfigure(IDictionary<string, string> parametersSet)
        {
            this.file = File.OpenText(parametersSet["source-location"]);
        }
    }
}