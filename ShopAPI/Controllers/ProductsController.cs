using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.HttpClients;
using ShopAPI.Models;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductServiceClient _serviceClient;

        public ProductsController()
        {
            _serviceClient = new ProductServiceClient();
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _serviceClient.GetProducts();
        }

        [HttpGet("{id}")]
        public async Task<Product> GetProductAsync(int id)
        {
            return await _serviceClient.GetProduct(id);
        }
    }
}