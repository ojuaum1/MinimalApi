using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Domains;
using MinimalAPI.Services;
using MongoDB.Driver;

namespace MinimalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMongoCollection<Client> _client;

        public ClientController(MongoDbService mongoDbService)
        {
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
        }

        [HttpGet]
        public async Task<ActionResult<List<Client>>> Get()
        {
            try
            {
                var clients = await _client.Find(FilterDefinition<Client>.Empty).ToListAsync();
                return Ok(clients);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Client client)
        {
            try
            {
                await _client.InsertOneAsync(client);
                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _client.DeleteOneAsync(x => x.Id == id);
                if (result.DeletedCount == 0)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetById(string id)
        {
            try
            {
                var client = await _client.Find(x => x.Id == id).FirstOrDefaultAsync();
                return client != null ? Ok(client) : NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Client updatedClient)
        {
            try
            {
                var filter = Builders<Client>.Filter.Eq(x => x.Id, id);
                var result = await _client.ReplaceOneAsync(filter, updatedClient);

                if (result.MatchedCount == 0)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
