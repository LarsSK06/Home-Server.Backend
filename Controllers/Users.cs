using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using HomeServer.Models;
using HomeServer.Data;
using Microsoft.AspNetCore.Authorization;

namespace HomeServer.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase{

    private readonly IMongoCollection<User>? _users;

    public UsersController(MongoDBService mongoDBService){
        _users = mongoDBService.Database?.GetCollection<User>("users");
    }

    [HttpGet, Authorize]
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

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<PublicUser>> DeleteUser(int id){
        if(_users is null)
            return NotFound();
        
        FilterDefinition<User>? filter = Builders<User>.Filter.Eq(i => i.Id, id);
        IAsyncCursor<User>? cursor = await _users.FindAsync(filter);
        User? first = await cursor.FirstOrDefaultAsync();

        if(first is null)
            return NotFound();

        DeleteResult? result = await _users.DeleteOneAsync(filter);

        return result.DeletedCount > 0
            ? Ok(first.ToPublic())
            : BadRequest();
    }

}