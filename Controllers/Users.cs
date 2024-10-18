using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using APIKvihaugenEngine.Entities;
using APIKvihaugenEngine.Data;

namespace APIKvihaugenEngine.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase{

    private readonly IMongoCollection<User>? _users;

    public UsersController(MongoDBService mongoDBService){
        _users = mongoDBService.Database?.GetCollection<User>("users");
    }

    [HttpGet]
    public async Task<IEnumerable<User>> GetUsers(){
        return
            await _users
                .Find(FilterDefinition<User>.Empty)
                .ToListAsync();
    }

    [HttpGet("{id}")]
    public ActionResult<User> GetUser(string id){
        FilterDefinition<User> filter = Builders<User>.Filter.Eq(i => i.Id, id);
        User user = _users.Find(filter).FirstOrDefault();

        return user is not null
            ? Ok(user)
            : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser(User user){
        FilterDefinition<User> filter = Builders<User>.Filter.Eq(i => i.Email, user.Email);

        if(await _users.Find(filter).CountDocumentsAsync() > 0)
            return Conflict();

        await _users!.InsertOneAsync(user);

        return CreatedAtAction(
            nameof(GetUser),
            new { email = user.Email },
            user
        );
    }

}