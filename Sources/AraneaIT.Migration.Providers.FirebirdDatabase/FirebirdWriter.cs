using System.Collections.Generic;
using System.Xml;
using FirebirdSql.Data.FirebirdClient;
using NLog;
using System;

namespace AraneaIT.Migration.Engine.XML
{
	[Writer("firebird")]
	internal sealed class FirebirdWriter : IWriter
	{
		public FirebirdWriter() { }

		public void Configure(IDictionary<string, string> parametersSet, IEntityDefinition entityDefinition)
		{
			var username = parametersSet.ContainsKey("username") ? parametersSet["username"] : "sysdba";
			var password = parametersSet.ContainsKey("password") ? parametersSet["password"] : "masterkey";
			var localhost = parametersSet.ContainsKey("localhost") ? parametersSet["localhost"] : "localhost";
			var charSet = parametersSet.ContainsKey("charset") ? parametersSet["charset"] : "UTF-8";
			var dbLocation = parametersSet["source-location"];

			var connectionString = String.Format(
				"User={0};Password={1};DataSource={2};Database={3};Charset={4};Dialect=3;",
				username,
				password,
				localhost,
				dbLocation,
				charSet
			);

			_logger.Info("FirebirdReader: connection string => {0}", connectionString);

			_connection = new FbConnection(connectionString);
			_tabelName = parametersSet["entity-name"];
			_entityDefinition = entityDefinition;
			_connection.Open();
		}

		public void SaveEntity(Entity currentEntity)
		{
			throw new System.NotImplementedException();
		}

		public void Dispose()
		{
			if (_connection != null)
			{
				_connection.Close();
				_connection.Dispose();
			}
		}

		private FbConnection _connection;
		private string _tabelName;
		private IEntityDefinition _entityDefinition;

		private static Logger _logger = LogManager.GetCurrentClassLogger();
	}
}