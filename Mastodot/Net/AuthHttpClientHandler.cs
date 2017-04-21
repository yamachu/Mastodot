using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mastodot.Net
{
    internal class AuthHttpClientHandler : DelegatingHandler
    {
        private string AccessToken;

        public AuthHttpClientHandler(string accessToken)
            : this(new HttpClientHandler(), accessToken)
        {
        }

        public AuthHttpClientHandler(HttpMessageHandler handler, string accessToken)
            : base(handler)
        {
            AccessToken = accessToken;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Authorization", $"Bearer {AccessToken}");
            return base.SendAsync(request, cancellationToken);
        }
    }
}
