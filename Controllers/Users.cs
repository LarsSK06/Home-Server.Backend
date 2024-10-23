using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using HomeServer.Models;
using HomeServer.Data;
using HomeServer.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace HomeServer.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase{

    private readonly IMongoCollection<User>? _users;
    private readonly IConfiguration _config;

    public UsersController(MongoDBService mongoDBService, IConfiguration configuration){
        _users = mongoDBService.Database?.GetCollection<User>("users");
        _config = configuration;
    }

    [HttpGet, Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<PublicUser>>> GetUsers(){
        if(_users is null)
            return NotFound();

        FilterDefinition<User>? filter = FilterDefinition<User>.Empty;
        IAsyncCursor<User>? cursor = await _users.FindAsync(filter);
        List<User>? users = await cursor.ToListAsync();
        List<PublicUser> publicUsers = [];

        foreach(User i in users)
            publicUsers.Add(i.ToPublic());

        return Ok(publicUsers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PublicUser>> GetUser(int id){
        if(_users is null)
            return NotFound();
        
        FilterDefinition<User>? filter = Builders<User>.Filter.Eq(i => i.Id, id);
        IAsyncCursor<User>? cursor = await _users.FindAsync(filter);
        User? first = await cursor.FirstOrDefaultAsync();

        if(first is null)
            return NotFound();

        return first.ToPublic();
    }

    [HttpPost]
    public async Task<ActionResult<PublicUser>> CreateUser(MutableUser data){
        if(_users is null)
            return NotFound();
        
        FilterDefinition<User> conflictedUsersFilter =
            Builders<User>.Filter.Eq(i => i.Email, data.Email);

        IAsyncCursor<User>? conflictedUsersCursor =
            await _users.FindAsync(conflictedUsersFilter);

        List<User> conflictedUsers = await conflictedUsersCursor.ToListAsync();

        if(conflictedUsers.Count > 0)
            return Conflict();

        User user = new(){
            Id = Generator.GetEpoch(),
            Name = data.Name,
            Password = BCrypt.Net.BCrypt.HashPassword(data.Password, 15),
            Email = data.Email,
            Admin = data.Email.Equals("lars@kvihaugen.no")
        };

        await _users.InsertOneAsync(user);

        return CreatedAtAction(
            nameof(GetUser),
            new { id = user.Id },
            user.ToPublic()
        );
    }

    [HttpPost("LogIn")]
    public async Task<ActionResult<PublicUser>> LogIn(Credentials credentials){
        if(_users is null)
            return NotFound();

        FilterDefinition<User>? filter = Builders<User>.Filter.Eq(i => i.Email, credentials.Email);
        IAsyncCursor<User>? users = await _users.FindAsync(filter);
        User? user = await users.FirstOrDefaultAsync();

        if(user is null)
            return BadRequest();

        if(!BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password))
            return Unauthorized();
        
        string token = JWT.CreateToken(_config, user.ToPublic());
        
        return Ok(token);
    }

}

public class Credentials{
    public required string Email { get; set; }
    public required string Password { get; set; }
}