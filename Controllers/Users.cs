using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using HomeServer.Models;
using HomeServer.Data;

namespace HomeServer.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase{

    public readonly IMongoCollection<User>? _users;

    public UsersController(MongoDBService mongoDBService){
        _users = mongoDBService.Database?.GetCollection<User>("users");
    }

    [HttpGet]
    public async Task<IEnumerable<PublicUser>> GetUsers(){
        List<User>? users = await (await _users.FindAsync(FilterDefinition<User>.Empty)).ToListAsync();
        List<PublicUser> publicUsers = new();

        foreach(User i in users)
            publicUsers.Add(new PublicUser{
                Id = i.Id,
                Name = i.Name,
                Email = i.Email,
                Admin = i.Admin
            });
        
        return publicUsers;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PublicUser>> GetUser(int id){
        if(_users is null)
            return NotFound();
        
        FilterDefinition<User>? filter = Builders<User>.Filter.Eq(i => i.Id, id);
        IAsyncCursor<User>? users = await _users.FindAsync(filter);
        User? user = await users.FirstOrDefaultAsync();

        if(user is null)
            return NotFound();

        return user.ToPublic();
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser(MutableUser data){
        if(_users is null)
            return NotFound();
        
        IAsyncCursor<User>? conflictedUsersCursor = await _users.FindAsync(Builders<User>.Filter.Eq(i => i.Email, data.Email));
        List<User> conflictedUsers = await conflictedUsersCursor.ToListAsync();

        if(conflictedUsers.Count() > 0)
            return Conflict();

        User user = new User{
            Id = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds,
            Name = data.Name,
            Password = data.Password, // hash
            Email = data.Email,
            Admin = data.Email.Equals(data.Email)
        };

        await _users.InsertOneAsync(user);

        return CreatedAtAction(
            nameof(GetUser),
            new { id = user.Id },
            user
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
            return NotFound();

        if(user.Password != credentials.Password)
            return Unauthorized();
        
        return Ok(user.ToPublic());
    }

}

public class Credentials{
    public required string Email { get; set; }
    public required string Password { get; set; }
}