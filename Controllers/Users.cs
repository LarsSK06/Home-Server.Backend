using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using HomeServer.Models;
using HomeServer.Data;

namespace HomeServer.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase{

    private readonly IMongoCollection<User>? _users;

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

        if(filter is null)
            return NotFound();

        IAsyncCursor<User>? users = await _users.FindAsync(filter);

        if(users is null)
            return NotFound();

        User? user = await users.FirstOrDefaultAsync();

        if(user is null)
            return NotFound();

        return user.ToPublic();
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser(MutableUser data){
        if(_users is null)
            return NotFound();

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

}