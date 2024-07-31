using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Domains;
using MinimalAPI.Services;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MinimalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// Armazena os dados de acesso da collection
        /// </summary>
        /// <param name="mongoDbService"></param>
        private readonly IMongoCollection<Product> _product;

        /// <summary>
        /// Construtor que recebe como dependência o objeto da classe MongoDbService
        /// </summary>
        /// <param name="mongoDbService"></param>
        public ProductController(MongoDbService mongoDbService)
        {
            //obtem a collection "product"
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                var product = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();

                return Ok(product);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            try
            {
                await _product.InsertOneAsync(product);

                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("_id")]
        public async Task<IActionResult> Delete(string _id)
        {
            try
            {
                var result = await _product.DeleteOneAsync(product => product.Id == _id);

                return StatusCode(204);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("_id")]
        public async Task<ActionResult<List<Product>>> GetById(string _id)
        {
            try
            {
                var product = await _product.Find(product => product.Id == _id).ToListAsync();

                return product is not null ? Ok(product) : NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut("_id")]
        public async Task<IActionResult> Update(Product updatedProduct)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(x => x.Id, updatedProduct.Id);

                await _product.ReplaceOneAsync(filter, updatedProduct);

                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}