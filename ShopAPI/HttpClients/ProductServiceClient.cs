using ShopAPI.Infrastructure;
using ShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShopAPI.HttpClients
{
    public class ProductServiceClient
    {
        private HttpClient _client;

        public ProductServiceClient()
        {
            _client = CustomHttpClientFactory.CreateHttpClientWithoutSslValidation();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var response = await _client.GetAsync(SERVICES_URLS.ProductService.Products);

            return await response.Content.ReadAsAsync<IEnumerable<Product>>();
        }

        public async Task<Product> GetProduct(int id)
        {
            var response = await _client.GetAsync(SERVICES_URLS.ProductService.Products + id);

            return await response.Content.ReadAsAsync<Product>();
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var response = await _client.DeleteAsync(SERVICES_URLS.ProductService.Products + id);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
