using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Domains;
using MinimalAPI.Services;
using MongoDB.Driver;

namespace MinimalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _user;

        public UserController(MongoDbService mongoDbService)
        {
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User is null.");
            }

            try
            {
                await _user.InsertOneAsync(user);
                return StatusCode(StatusCodes.Status201Created, user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                var users = await _user.Find(FilterDefinition<User>.Empty).ToListAsync();
                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            try
            {
                var user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] User updatedUser)
        {
            // Verifica se o objeto User é nulo ou se o ID passado não corresponde ao ID do User
            if (updatedUser == null || id != updatedUser.Id)
            {
                return BadRequest("User data is invalid.");
            }

            try
            {
                
                var filter = Builders<User>.Filter.Eq(x => x.Id, id);

               
                var result = await _user.ReplaceOneAsync(filter, updatedUser);

               
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _user.DeleteOneAsync(x => x.Id == id);

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
    }
}
