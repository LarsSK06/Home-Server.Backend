using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using HomeServer.Models;
using HomeServer.Data;
using HomeServer.Utilities;

namespace HomeServer.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase{

    private readonly MongoDBService _mongoService;
    private readonly IMongoCollection<User>? _users;
    private readonly IConfiguration _config;

    public AuthController(MongoDBService mongoDBService, IConfiguration configuration){
        _mongoService = mongoDBService;
        _users = mongoDBService.Database?.GetCollection<User>("users");
        _config = configuration;
    }

    [HttpPost("SignIn")]
    public async Task<ActionResult<PublicUser>> SignIn(Credentials credentials){
        if(_users is null)
            return NotFound();

        FilterDefinition<User>? filter = Builders<User>.Filter.Eq(i => i.Email, credentials.Email);
        IAsyncCursor<User>? users = await _users.FindAsync(filter);
        User? user = await users.FirstOrDefaultAsync();

        if(user is null)
            return BadRequest();

        if(!BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password))
            return Unauthorized();
        
        string token = JWT.CreateToken(_config, user);
        
        return Ok(token);
    }

    [HttpPost("SignUp")]
    public async Task<ActionResult<PublicUser>> SignUp(MutableUser data){
        if(_users is null)
            return NotFound();
        
        IAsyncCursor<User> conflictsCursor = await _users.FindAsync(i => data.Email.Equals(i.Email));
        List<User>? conflicts = await conflictsCursor.ToListAsync();

        if(conflicts.Count > 0)
            return Conflict();

        User? user = new(){
            Id = Generator.GetEpoch(),
            Name = data.Name,
            Password = BCrypt.Net.BCrypt.HashPassword(data.Password, 14),
            Email = data.Email,
            Admin = data.Email.Equals("lars@kvihaugen.no")
        };

        await _users.InsertOneAsync(user);

        string token = JWT.CreateToken(_config, user);

        return Ok(token);
    }

}

public class Credentials{
    public required string Email { get; set; }
    public required string Password { get; set; }
}