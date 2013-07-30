// <copyright file="PutWriter.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Providers.WebAPI
{
    using AraneaIT.Migration.Engine;
    using AraneaIT.Migration.Engine.Actions;
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// WebApi writer provider
    /// </summary>
    [Writer("WebApi-Throttled")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class ThrottledWriter : Writer
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
        /// The chunk size
        /// </summary>
        private int chunkSize;

        /// <summary>
        /// The wait for
        /// </summary>
        private int waitFor;
        private const int Timeout = 300000;

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

            var request = BuildRequest();

            var json = JsonConvert.SerializeObject(dtoList);
            var jsonBytes = Encoding.UTF8.GetBytes(json);
            var lenght = jsonBytes.Length;
            request.ContentLength = jsonBytes.Length;

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    var position = 0;
                    var currentChunkSize = this.chunkSize;
                    var timer = new System.Diagnostics.Stopwatch();

                    while (position < lenght)
                    {
                        timer.Start();

                        if (position + currentChunkSize > lenght)
                        {
                            currentChunkSize = lenght - position;
                        }

                        stream.Write(jsonBytes, position, currentChunkSize);
                        position += currentChunkSize;

                        timer.Stop();

                        if (timer.ElapsedMilliseconds < this.waitFor)
                        {
                            var sleepFor = (int)(this.waitFor - timer.ElapsedMilliseconds);
                            Thread.Sleep(sleepFor);
                        }
                    }

                    stream.Flush();
                    stream.Close();


                    using (var response = request.GetResponse() as HttpWebResponse)
                    {

                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            var resultItem = string.Empty;
                            using (var streamReader = new StreamReader(response.GetResponseStream()))
                            {
                                resultItem = streamReader.ReadToEnd();
                            }
                            var message = ServerErrorFormatter.GetMessage(resultItem);
                            this.Logger.Error(message);
                            throw new ActionInternalException(message);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                this.Logger.Fatal(exp.Message);
            }
            //var result = this.client.PutAsJsonAsync(this.apiURI, dtoList).Result;

            //if (!result.IsSuccessStatusCode)
            //{
            //    var message = ServerErrorFormatter.GetMessage(result.Content.ReadAsStringAsync().Result);
            //    this.Logger.Error(message);
            //    throw new ActionInternalException(message);
            //}

            //result.UpdateWithWebApiReturnValues(this.returnType, entities);
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
            this.chunkSize = parametersSet.ContainsKey("chunk-size") ? int.Parse(parametersSet["chunk-size"]) : 1024;
            this.waitFor = parametersSet.ContainsKey("wait-for") ? int.Parse(parametersSet["wait-for"]) : 1000;

            this.client = new HttpClient
            {
                BaseAddress = new Uri(parametersSet["server-uri"]),
                MaxResponseContentBufferSize = 2147483646,
                Timeout = TimeSpan.FromMinutes(parametersSet.GetValueOrDefault("timeout", "2").ToInt32())
            };
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Builds the request.
        /// </summary>
        /// <returns></returns>
        private HttpWebRequest BuildRequest()
        {
            var request = WebRequest.Create(this.client.BaseAddress + this.apiURI) as HttpWebRequest;
            request.SendChunked = true;
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.AllowWriteStreamBuffering = false;
            request.AllowReadStreamBuffering = false;
            request.ContinueTimeout = ThrottledWriter.Timeout;
            request.KeepAlive = true;
            request.ReadWriteTimeout = ThrottledWriter.Timeout * 3;
            request.Timeout = ThrottledWriter.Timeout;
            return request;
        }
    }
}