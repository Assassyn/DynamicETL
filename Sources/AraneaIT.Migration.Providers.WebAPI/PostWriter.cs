// <copyright file="PostWriter.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Providers.WebAPI
{
    using AraneaIT.Migration.Engine;
    using AraneaIT.Migration.Engine.Actions;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Net.Http;
    using System.Net.Http.Headers;

    /// <summary>
    /// Web Api writer provider
    /// </summary>
    [Writer("WebApi-Post")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class PostWriter : Writer
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
        /// Saves the entity.
        /// </summary>
        /// <param name="currentEntity">The current entity.</param>
        /// <exception cref="ActionInternalException">Server response code</exception>
        public async override void SaveEntity(Entity currentEntity)
        {
            var dto = this.requestType.CreateObjectBasedOnEntity(this.EntityDefinition, currentEntity);

            var result = await this.client.PostAsJsonAsync(this.apiURI, dto);

            if (!result.IsSuccessStatusCode)
            {
                var message = ServerErrorFormatter.GetMessage(result.Content.ReadAsStringAsync().Result);
                this.Logger.Error(message);
                throw new ActionInternalException(message);
            }

            if (this.returnType != null)
            {
                var returnObject = result.Content.ReadAsAsync(this.returnType).Result;

                currentEntity.UpdateValues(returnObject);
            }
        }

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

            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(parametersSet["server-uri"]);
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}