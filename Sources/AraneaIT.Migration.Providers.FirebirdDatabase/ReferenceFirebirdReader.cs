// <copyright file="ReferenceFirebirdReader.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Providers.FirebirdDatabase
{
    using AraneaIT.Migration.Engine;
    using AraneaIT.Migration.Engine.Actions;
    using FirebirdSql.Data.FirebirdClient;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Firebird reader which read one line every request
    /// </summary>
    [Reader("Firebird-Reference")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class ReferenceFirebirdReader : Reader
    {
        /// <summary>
        /// The entity name
        /// </summary>
        private string entityName;

        /// <summary>
        /// The value
        /// </summary>
        private Dictionary<string, object> value;

        /// <summary>
        /// The connection
        /// </summary>
        private FbConnection connection;

        /// <summary>
        /// Reads the specified working entity.
        /// </summary>
        /// <param name="workingEntity">The working entity.</param>
        /// <returns>
        /// Populated entity object
        /// </returns>
        public override Entity Read(Entity workingEntity)
        {
            try
            {
                using (var command = this.connection.CreateCommand())
                {
                    command.CommandText = string.Format(
                        "SELECT {0} FROM {1}{2};",
                        string.Join(",", this.EntityDefinition.PropertiesDefinition.Select(x => x.SourceName)),
                        this.entityName,
                        this.GetWhereClause(this.EntityDefinition, workingEntity));

                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        this.value = new Dictionary<string, object>();

                        foreach (var definition in this.EntityDefinition.PropertiesDefinition)
                        {
                            this.value[definition.SourceName] = reader[definition.SourceName];
                        }
                    }
                }

                this.Logger.Trace("FirebirdReader: Started loadieng file");

                this.PerformedReading = true;

                foreach (var definition in this.EntityDefinition.PropertiesDefinition)
                {
                    this.Logger.Trace(
                        "FirebirdReader: \tproperty =>{0}<= of type =>{1}<= with value =>{2}",
                        definition.Name,
                        definition.Type,
                        this.value[definition.SourceName]);

                    workingEntity.AddProperty(
                        definition.Name,
                        definition.Type,
                        this.value[definition.SourceName]);
                }

                return workingEntity;
            }
            catch (FbException exp)
            {
                this.Logger.Fatal(exp);
                return new Entity();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (this.connection != null)
            {
                this.connection.Dispose();
            }
        }

        /// <summary>
        /// Additional the configure.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        protected override void AdditionalConfigure(IDictionary<string, string> parametersSet)
        {
            this.entityName = parametersSet["entity-name"];
            var connectionStringBuilder = new FbConnectionStringBuilder
            {
                UserID = parametersSet.GetValueOrDefault("username", "sysdba"),
                Password = parametersSet.GetValueOrDefault("password", "masterkey"),
                DataSource = parametersSet.GetValueOrDefault("database-name", "localhost"),
                Database = parametersSet["source-location"],
                Charset = parametersSet.GetValueOrDefault("charset", "NONE"),
                Dialect = 3
            };

            var connectionString = connectionStringBuilder.ToString();

            this.Logger.Trace("FirebirdReader: connection string => {0}", connectionString);

            this.connection = new FbConnection(connectionString);

            this.connection.Open();
        }

        /// <summary>
        /// Gets the where clause.
        /// </summary>
        /// <param name="entityDefinition">The entity definition.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Where clouse for the Firebird Connection</returns>
        private string GetWhereClause(IEntityDefinition entityDefinition, Entity entity)
        {
            var result = new StringBuilder(2048);

            if (this.Conditions != null && this.Conditions.Count() > 0)
            {
                result.Append(" WHERE ");

                foreach (var condition in this.Conditions)
                {
                    result.AppendFormat(
                        " {0} {1} {2}",
                        condition.SearchKey,
                        condition.Comparer,
                        condition.GetValue(entity));
                }
            }

            return result.ToString();
        }
    }
}