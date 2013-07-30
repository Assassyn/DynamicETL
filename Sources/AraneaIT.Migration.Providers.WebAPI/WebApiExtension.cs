// <copyright file="WebApiExtension.cs" company="Aranea IT Ltd">
//     Copyright (c) Aranea IT Ltd. All rights reserved.
// </copyright>
// <author>Szymon Sasin</author>

namespace AraneaIT.Migration.Providers.WebAPI
{
    using AraneaIT.Migration.Engine;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;

    /// <summary>
    /// Contains shared functionality
    /// </summary>
    internal static class WebApiExtension
    {
        /// <summary>
        /// Updates the with web API return values.
        /// </summary>
        /// <param name="webApiResult">The web API result.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="entities">The entities.</param>
        public static void UpdateWithWebApiReturnValues(this HttpResponseMessage webApiResult, Type returnType, IEnumerable<Entity> entities)
        {
            if (webApiResult.StatusCode != HttpStatusCode.NoContent && returnType != null)
            {
                var returnCollection = webApiResult.Content.ReadAsAsync(returnType).Result as IEnumerable<object>;

                for (int index = 0; index < entities.Count(); ++index)
                {
                    entities.ElementAt(index).UpdateValues(returnCollection.ElementAt(index));
                }
            }
        }
    }
}