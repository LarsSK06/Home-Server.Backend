using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using HomeServer.Models;
using HomeServer.Data;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using HomeServer.Utilities;
using HomeServer.Enums;

namespace HomeServer.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentsController : ControllerBase{

    private readonly IMongoCollection<Document>? _documents;
    private readonly IMongoCollection<User>? _users;
    private readonly IConfiguration _config;

    public DocumentsController(MongoDBService mongoDBService, IConfiguration configuration){
        _documents = mongoDBService.Database?.GetCollection<Document>("documents");
        _users = mongoDBService.Database?.GetCollection<User>("users");
        _config = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<List<PublicDocument>>> GetDocuments(){
        if(_documents is null)
            return NotFound();

        if(_users is null)
            return NotFound();

        FilterDefinition<Document>? filter = FilterDefinition<Document>.Empty;
        IAsyncCursor<Document>? cursor = await _documents.FindAsync(filter);
        List<Document>? documents = await cursor.ToListAsync();
        List<PublicDocument> visibleDocuments = [];
        JwtSecurityToken? token = JWT.ReadToken(Request);

        foreach(Document i in documents){
            bool visible =
                i.Publicity == PublicityStatus.Public
                || JWT.CompareUserId(token, i.OwnerId);

            if(visible)
                visibleDocuments.Add(await i.ToPublic(_users));
        }

        return Ok(visibleDocuments);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PublicDocument>> CreateDocument(MutableDocument data){
        if(_documents is null)
            return NotFound();

        if(_users is null)
            return NotFound();
        
        int? ownerId = JWT.GetTokenUserId(JWT.ReadToken(Request));

        if(ownerId is null)
            return BadRequest();
        
        Document? document = new(){
            Id = Generator.GetEpoch(),
            Name = data.Name,
            Lines = data.Lines,
            Publicity = data.Publicity,
            OwnerId = (int)ownerId
        };

        await _documents.InsertOneAsync(document);

        return CreatedAtAction(
            nameof(GetDocument),
            new{ id = document.Id },
            await document.ToPublic(_users)
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<List<PublicDocument>>> GetDocument(int id){
        if(_documents is null)
            return NotFound();

        if(_users is null)
            return NotFound();

        IAsyncCursor<Document>? cursor = await _documents.FindAsync(i => i.Id == id);
        Document? document = await cursor.FirstOrDefaultAsync();
        JwtSecurityToken? token = JWT.ReadToken(Request);

        return JWT.CompareUserId(token, document.OwnerId)
            ? Ok(document.ToPublic(_users))
            : NotFound();
    }

}