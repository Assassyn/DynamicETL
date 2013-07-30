// <copyright file="FirebirdReader.cs" company="Aranea IT Ltd">
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
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Firebird Reader
    /// </summary>
    [DebuggerDisplay("Firebird reader:{entityName}")]
    [Reader("firebird")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class FirebirdReader : Reader
    {
        /// <summary>
        /// The where joints
        /// </summary>
        private static string[] WhereJoints = { "AND", "OR" };

        /// <summary>
        /// The connection
        /// </summary>
        private FbConnection connection;

        /// <summary>
        /// The reader
        /// </summary>
        private FbDataReader reader;

        /// <summary>
        /// The entity name
        /// </summary>
        private string entityName;

        /// <summary>
        /// The command
        /// </summary>
        private FbCommand command;

        /// <summary>
        /// Reads the specified working entity.
        /// </summary>
        /// <param name="workingEntity">The working entity.</param>
        /// <returns>
        /// Populated entity object
        /// </returns>
        public override Entity Read(Entity workingEntity)
        {
            this.Logger.Trace("FirebirdReader: Started loading file");

            this.CreateReader(workingEntity);

            this.PerformedReading = this.reader.Read();

            if (this.PerformedReading)
            {
                foreach (var definition in EntityDefinition.PropertiesDefinition)
                {
                    var recordValue = this.reader[definition.Name];

                    this.Logger.Trace(
                        "FirebirdReader:\tproperty {0} of type {1} with value {2}",
                        definition.Name,
                        definition.Type,
                        recordValue);

                    workingEntity.AddProperty(
                        definition.Name,
                        definition.Type,
                        recordValue);
                }
            }

            return workingEntity;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (this.reader != null)
            {
                this.reader.Close();
                this.reader.Dispose();
            }

            if (this.command != null)
            {
                this.command.Dispose();
            }

            if (this.connection != null)
            {
                this.connection.Close();
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
        /// Creates the reader.
        /// </summary>
        /// <param name="entity">The entity.</param>
        private void CreateReader(Entity entity)
        {
            if (this.reader == null)
            {
                var select = this.GetSelect();
                var where = this.GetWhere(this.EntityDefinition, entity);
                var join = this.GetJoin('b', select);

                this.command = this.connection.CreateCommand();
                this.command.CommandText = string.Format(
                    "SELECT {0} FROM {1} a {2}{3};",
                    select,
                    this.entityName,
                    join,
                    where);

                this.reader = this.command.ExecuteReader();
            }
        }

        /// <summary>
        /// Gets the select.
        /// </summary>
        /// <returns></returns>
        private StringBuilder GetSelect()
        {
            var result = new StringBuilder();

            foreach (var property in this.EntityDefinition.PropertiesDefinition)
            {
                result.AppendFormat(
                    ",a.{0} as {1}",
                    property.SourceName,
                    property.Name);
            }

            return result.Remove(0, 1);
        }

        /// <summary>
        /// Gets the where clause.
        /// </summary>
        /// <param name="entityDefinition">The entity definition.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// SQL Where clause
        /// </returns>
        private  StringBuilder GetWhere(IEntityDefinition entityDefinition, Entity entity)
        {
            var result = new StringBuilder(2048);

            if (this.Conditions != null && this.Conditions.Count() > 0)
            {
                result.Append(" WHERE ");

                foreach (var condition in this.Conditions)
                {
                    if (FirebirdReader.WhereJoints.Contains(condition.SearchKey, StringComparer.OrdinalIgnoreCase))
                    {
                        result.AppendFormat(" {0} ", condition.SearchKey);
                    }
                    else
                    {
                        result.AppendFormat(
                            " a.{0} {1} {2}",
                            condition.SearchKey,
                            condition.Comparer,
                            condition.GetValue(entity));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the join.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="select">The select.</param>
        /// <returns>String builder with joins </returns>
        private StringBuilder GetJoin(char prefix, StringBuilder select)
        {
            var result = new StringBuilder();
            var joinPrefix = prefix++;

            if (this.Joins != null)
            {
                foreach (var join in this.Joins)
                {
                    result.AppendFormat(
                        " join {0} {1} on {1}.{2} = {3} ",
                        join.EntityName,
                        joinPrefix,
                        join.EntityKey,
                        join.MasterKey);

                    foreach (var property in join.EntityDefinition.PropertiesDefinition)
                    {
                        select.AppendFormat(",{0}.{1} as {2}", joinPrefix, property.SourceName, property.Name);

                        this.EntityDefinition.AddProperty(property);
                    }

                    joinPrefix++;
                }
            }

            return result;
        }
    }
}