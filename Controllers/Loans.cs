using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using HomeServer.Models;
using HomeServer.Data;
using HomeServer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace HomeServer.Controllers;

[EnableCors]
[ApiController]
[Route("[controller]")]
public class LoansController : ControllerBase{

    public readonly IMongoCollection<User>? _users;
    public readonly IMongoCollection<Loan>? _loans;

    public LoansController(MongoDBService mongoDBService){
        _users = mongoDBService.Database?.GetCollection<User>("users");
        _loans = mongoDBService.Database?.GetCollection<Loan>("loans");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PublicLoan>>> GetLoans(){
        if(_users is null)
            return NotFound();

        if(_loans is null)
            return NotFound();

        FilterDefinition<Loan>? filter = FilterDefinition<Loan>.Empty;
        IAsyncCursor<Loan>? cursor = await _loans.FindAsync(filter);
        List<Loan>? loans = await cursor.ToListAsync();
        List<PublicLoan> publicLoans = new();

        foreach(Loan i in loans)
            publicLoans.Add(await i.ToPublic(_users));

        return Ok(publicLoans);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PublicLoan>> GetLoan(int id){
        if(_users is null)
            return NotFound();
        
        if(_loans is null)
            return NotFound();
        
        FilterDefinition<Loan>? filter = Builders<Loan>.Filter.Eq(i => i.Id, id);
        IAsyncCursor<Loan>? cursor = await _loans.FindAsync(filter);
        Loan? first = await cursor.FirstOrDefaultAsync();

        if(first is null)
            return NotFound();

        return await first.ToPublic(_users);
    }

    [HttpPost]
    public async Task<ActionResult<PublicUser>> CreateLoan(MutableLoan data){
        if(_users is null)
            return NotFound();

        if(_loans is null)
            return NotFound();

        Loan loan = new Loan{
            Id = Generator.GetEpoch(),
            Name = data.Name,
            Subject = data.Subject,
            Item = data.Item,
            Object = data.Object,
            OwnerId = 1729504619 // SET OWNER ID
        };

        await _loans.InsertOneAsync(loan);

        return CreatedAtAction(
            nameof(GetLoan),
            new { id = loan.Id },
            await loan.ToPublic(_users)
        );
    }

}