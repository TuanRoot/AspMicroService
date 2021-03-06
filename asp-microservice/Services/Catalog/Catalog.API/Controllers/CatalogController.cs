using Catalog.API.Entities;
using Catalog.API.Repositoies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _producRepository;
        private readonly ILogger<CatalogController> _logger;
        public CatalogController(IProductRepository producRepository, ILogger<CatalogController> logger)
        {
            _producRepository = producRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _producRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name ="GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductById(string id)
        {
            var product = await _producRepository.GetProduct(id);
            if(product == null)
            {
                _logger.LogError($"Production with id : {id}, not found ");
                return NotFound();
            }
            return Ok(product);
        }

        [Route("[action]/{category}", Name ="GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await _producRepository.GetProductByCategory(category);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateProduct([FromBody] Product product)
        {
            await _producRepository.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product)
        {
            await _producRepository.UpdateProduct(product);
            return Ok();
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _producRepository.DeleteProduct(id));
        }

    }
}
