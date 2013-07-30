// <copyright file="XmlConfigurator.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Engine.Configuration
{
    using AraneaIT.Migration.Engine.Proceses;
    using NLog;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Default IConfiguration implementation, it construct process based on provided XML file.
    /// </summary>
    public class XmlConfigurator : Configurator
    {
        /// <summary>
        /// The file name
        /// </summary>
        public static string FileName = "filename";

        /// <summary>
        /// The configuration document
        /// </summary>
        private XDocument configurationDocument;

        /// <summary>
        /// The logger
        /// </summary>
        private Logger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlConfigurator"/> class.
        /// </summary>
        public XmlConfigurator()
        {
            this.logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Configures the specified configuration set.
        /// </summary>
        /// <param name="configurationSet">The configuration set.</param>
        public override void Configure(IDictionary<string, string> configurationSet)
        {
            try
            {
                this.configurationDocument = XDocument.Load(File.OpenText(configurationSet[XmlConfigurator.FileName]));
                this.IsConfigured = true;
            }
            catch (XmlException exp)
            {
                this.IsConfigured = false;
                this.logger.Fatal(exp);
            }
        }

        /// <summary>
        /// Gets the processes.
        /// </summary>
        /// <param name="mefContainer">The mef container.</param>
        /// <returns></returns>
        protected override IEnumerable<Process> GetProcesses(CompositionContainer mefContainer)
        {
            return this.configurationDocument
                .Element(XName.Get("configuration"))
                .Element(XName.Get("processes"))
                .Elements(XName.Get("process"))
                .Select(x => CreateProcess(mefContainer, x));
        }

        /// <summary>
        /// Creates the process.
        /// </summary>
        /// <param name="objectContainer">The object container.</param>
        /// <param name="processConfigurationDefinition">The process configuration definition.</param>
        /// <returns></returns>
        private Process CreateProcess(CompositionContainer objectContainer, XElement processConfigurationDefinition)
        {
            var processName = processConfigurationDefinition.GetAttributeValueOrDefault("process-name", "Default");
            var process = objectContainer.GetExportedValueOrDefault<Process>(processName);

            process.ReaderCollection = processConfigurationDefinition.Elements(XName.Get("reader")).Select(node => this.GetReader(objectContainer, node)).ToArray();
            process.ConverterCollection = processConfigurationDefinition.Elements(XName.Get("converter")).Select(reader => this.GetAction<IConverter>(objectContainer, reader)).ToArray();
            process.WriterCollection = processConfigurationDefinition.Elements(XName.Get("writer")).Select(reader => this.GetAction<IWriter>(objectContainer, reader));

            process.Configure(
                processConfigurationDefinition
                    .Attributes()
                    .ToDictionary(
                        key => key.Name.LocalName,
                        value => value.Value));

            return process;
        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <typeparam name="TAction">The type of the action.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="node">The node.</param>
        /// <returns>
        /// generic action generated from XML node
        /// </returns>
        private TAction GetAction<TAction>(CompositionContainer container, XElement node)
            where TAction : IAction
        {
            var contractName = node.Attribute(XName.Get("type")).Value.ToLower();
            var action = container.GetExportedValueOrDefault<TAction>(contractName);

            //List<PropertyDescriptor> properties = new List<PropertyDescriptor>();

            //var entityNode = node.Element(XName.Get("entity"));
            //var entityType = entityNode.Attribute(XName.Get("type"));

            //if (entityType != null)
            //{
            //    var ignoreSourceNameAttrubute = entityNode.Attribute("ignore-source-name");
            //    var ignoreSourceName = false;

            //    if (ignoreSourceNameAttrubute != null)
            //    {
            //        ignoreSourceName = ignoreSourceNameAttrubute.Value.ToBoolean();
            //    }

            //    properties.AddRange(AttributeEntityDefinitionFactory.CreateEntityDefinition(entityType.Value, ignoreSourceName));
            //}

            //properties.AddRange(XMLEntityDefinitionFactory.CreateEntityDefinition(entityNode));

            action.Configure(
                this.GetConfigurationParameters(node),
                node.GetEntityDefinition());

            return action;
        }

        /// <summary>
        /// Gets the reader.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="node">The node.</param>
        /// <returns>
        /// reader generated from XML node
        /// </returns>
        private IReader GetReader(CompositionContainer container, XElement node)
        {
            var reader = this.GetAction<IReader>(container, node);

            if (node.Element("conditions") != null)
            {
                reader.SetReadConditions(node
                    .Element("conditions")
                    .Elements("condition")
                    .Select(conditionNode =>
                        new ReadCondition
                        {
                            SearchKey = conditionNode.GetAttributeValueOrDefault("key", string.Empty),
                            Comparer = conditionNode.GetAttributeValueOrDefault("comparer", string.Empty),
                            Value = conditionNode.GetAttributeValueOrDefault("value", string.Empty),
                            AddQuotes = conditionNode.GetAttributeValueOrDefault("add-quotes", false),
                            ValueFormat = conditionNode.GetAttributeValueOrDefault("value-format", "dd/mm/yyyy"),
                            QuoteCharacter = conditionNode.GetAttributeValueOrDefault("quote-character", "'"),
                        }));
            }

            var joinsNode = node.Elements(XName.Get("join"));
            if (joinsNode != null && joinsNode.Count() > 0)
            {
                reader.SetJoins(joinsNode.Select(x => new Join
                 {
                     EntityName = x.GetAttributeValueOrDefault("entity-name", string.Empty),
                     MasterKey = x.GetJoinOnAttribute("main-key"),
                     EntityKey = x.GetJoinOnAttribute("entity-key"),
                     EntityDefinition = x.GetEntityDefinition()
                 })
                 .ToArray()
                 );
            }


            return reader;
        }

        /// <summary>
        /// Gets the configuration parameters.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        /// dictionary of configuration parameters
        /// </returns>
        private Dictionary<string, string> GetConfigurationParameters(XElement node)
        {
            var configurationNode = node.Element(XName.Get("configurationParameters"));
            var configurationSet = new Dictionary<string, string>();

            if (configurationNode != null)
            {
                configurationSet = configurationNode.Elements(XName.Get("add")).ToDictionary(
                      x => x.Attribute(XName.Get("key")).Value,
                      x => GetValueForConfiguration(x));
            }

            return configurationSet;
        }

        /// <summary>
        /// Gets the value for configuration.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>
        /// NOde value of appropirate AppSettings value
        /// </returns>
        private static string GetValueForConfiguration(XElement x)
        {
            var configurationValue = x.Attribute(XName.Get("value")).Value;

            if (configurationValue.StartsWith("@{") && configurationValue.EndsWith("}"))
            {
                configurationValue = ConfigurationManager.AppSettings[configurationValue.Trim('@', '{', '}')];
            }

            return configurationValue;
        }
    }
}