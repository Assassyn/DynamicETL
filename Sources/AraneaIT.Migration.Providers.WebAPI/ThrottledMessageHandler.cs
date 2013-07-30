namespace AraneaIT.Migration.Providers.WebAPI
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class ThrottledMessageHandler : HttpClientHandler
    {
        public ThrottledMessageHandler()
        {

        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
        }
    }
}