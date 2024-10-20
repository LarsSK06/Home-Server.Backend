using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace HomeServer.Models;

public class User{

    [BsonId]
    [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string? MongoId { get; set; }

    [BsonElement("id"), BsonRepresentation(BsonType.Int32)]
    public required int Id { get; set; }

    [BsonElement("name"), BsonRepresentation(BsonType.String)]
    public required string Name { get; set; }

    [BsonElement("password"), BsonRepresentation(BsonType.String)]
    public required string Password { get; set; }

    [BsonElement("email"), BsonRepresentation(BsonType.String)]
    public required string Email { get; set; }

    [BsonElement("admin"), BsonRepresentation(BsonType.Boolean)]
    public required bool Admin { get; set; }

    [BsonElement("loans")]
    public required int[] Loans { get; set; }

    public async Task<PublicUser> ToPublic(IMongoCollection<Loan> loanCollection){
        List<PublicLoanEmbed> loans = new List<PublicLoanEmbed>{};

        foreach(int i in Loans){
            FilterDefinition<Loan> filter = Builders<Loan>.Filter.Eq(i => i.OwnerId, Id);
            IAsyncCursor<Loan> cursor = await loanCollection.FindAsync<Loan>(filter);
            Loan? first = await cursor.FirstOrDefaultAsync();

            if(first is not null)
                loans.Add(first.ToEmbed());
        }

        return new PublicUser{
            Id = Id,
            Name = Name,
            Email = Email,
            Admin = Admin,
            Loans = loans
        };
    }

    public PublicUserEmbed ToEmbed(){
        return new PublicUserEmbed{
            Id = Id,
            Name = Name,
            Email = Email,
            Admin = Admin
        };
    }

    public MutableUser ToMutable(){
        return new MutableUser{
            Name = Name,
            Password = Password,
            Email = Email
        };
    }

}

public class PublicUser : PublicUserEmbed{

    public required IEnumerable<PublicLoanEmbed> Loans { get; set; }

}

public class PublicUserEmbed{

    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required bool Admin { get; set; }

}

public class MutableUser{

    public required string Name { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }

}