using System;
using System.Net.Http;

namespace ShopAPI.Infrastructure
{
    public static class CustomHttpClientFactory
    {
        public static HttpClient CreateHttpClientWithoutSslValidation()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            return new HttpClient(handler);
        }
    }
}
