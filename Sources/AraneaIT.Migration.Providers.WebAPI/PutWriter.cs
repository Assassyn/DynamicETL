// <copyright file="PutWriter.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Providers.WebAPI
{
    using AraneaIT.Migration.Engine;
    using AraneaIT.Migration.Engine.Actions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    /// <summary>
    /// WebApi writer provider
    /// </summary>
    [Writer("WebApi-Put")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class PutWriter : Writer
    {
        /// <summary>
        /// The type of DTO object
        /// </summary>
        private Type requestType;

        /// <summary>
        /// The return type
        /// </summary>
        private Type returnType;

        /// <summary>
        /// The client
        /// </summary>
        private HttpClient client;

        /// <summary>
        /// The API URI
        /// </summary>
        private string apiURI;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (this.client != null)
            {
                this.client.Dispose();
            }
        }

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="currentEntity">The current entity.</param>
        /// <exception cref="ActionInternalException">Server response code</exception>
        public override void SaveEntity(Entity currentEntity)
        {
            var dto = this.requestType.CreateObjectBasedOnEntity(this.EntityDefinition, currentEntity);

            var result = this.client.PutAsJsonAsync(this.apiURI, dto).Result;

            if (!result.IsSuccessStatusCode)
            {
                var message = ServerErrorFormatter.GetMessage(result.Content.ReadAsStringAsync().Result);
                this.Logger.Error(message);
                throw new ActionInternalException(message);
            }

            if (result.StatusCode != HttpStatusCode.NoContent && this.returnType != null)
            {
                var returnObject = result.Content.ReadAsAsync(this.returnType).Result;

                currentEntity.UpdateValues(returnObject);
            }
        }

        /// <summary>
        /// Saves the batch.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <exception cref="ActionInternalException">Server response error</exception>
        public override void SaveBatch(IEnumerable<Entity> entities)
        {
            var dtoList = new ArrayList();

            foreach (var entity in entities)
            {
                dtoList.Add(this.requestType.CreateObjectBasedOnEntity(this.EntityDefinition, entity));
            }

            var result = this.client.PutAsJsonAsync(this.apiURI, dtoList).Result;

            if (!result.IsSuccessStatusCode)
            {
                var message = ServerErrorFormatter.GetMessage(result.Content.ReadAsStringAsync().Result);
                this.Logger.Error(message);
                throw new ActionInternalException(message);
            }

            result.UpdateWithWebApiReturnValues(this.returnType, entities);
        }

        /// <summary>
        /// Additional the configuration.
        /// </summary>
        /// <param name="parametersSet">The parameters set.</param>
        protected override void Configure(IDictionary<string, string> parametersSet)
        {
            this.requestType = Type.GetType(parametersSet["request-type"]);

            if (parametersSet.ContainsKey("return-type"))
            {
                this.returnType = Type.GetType(parametersSet["return-type"]);
            }

            this.apiURI = parametersSet["api-uri"];

            
            this.client = new HttpClient
            {
                BaseAddress = new Uri(parametersSet["server-uri"]),
                MaxResponseContentBufferSize = 2147483646,
                Timeout = TimeSpan.FromMinutes(parametersSet.GetValueOrDefault("timeout", "2").ToInt32())
            };

            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}