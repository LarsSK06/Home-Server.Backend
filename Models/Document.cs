using HomeServer.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace HomeServer.Models;

public class Document{

    [BsonId]
    [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string? MongoId { get; set; }

    [BsonElement("id"), BsonRepresentation(BsonType.Int32)]
    public required int Id { get; set; }

    [BsonElement("name"), BsonRepresentation(BsonType.String)]
    public required string Name { get; set; }

    [BsonElement("lines")]
    public required List<string> Lines { get; set; }

    [BsonElement("publicity"), BsonRepresentation(BsonType.String)]
    public required PublicityStatus Publicity { get; set; }

    [BsonElement("ownerId"), BsonRepresentation(BsonType.Int32)]
    public required int OwnerId { get; set; }

    public async Task<PublicDocument> ToPublic(IMongoCollection<User> users){
        IAsyncCursor<User>? cursor = await users.FindAsync(i => i.Id == OwnerId);
        User? user = await cursor.FirstOrDefaultAsync();

        return new PublicDocument{
            Id = Id,
            Name = Name,
            Lines = Lines,
            Publicity = Publicity,
            Owner = user.ToPublic()
        };
    }

    public MutableDocument ToMutable(){
        return new MutableDocument{
            Name = Name,
            Lines = Lines,
            Publicity = Publicity
        };
    }

}

public class PublicDocument{

    public required int Id { get; set; }
    public required string Name { get; set; }
    public required List<string> Lines { get; set; }
    public required PublicityStatus Publicity { get; set; }
    public required PublicUser Owner { get; set; }

}

public class MutableDocument{

    public required string Name { get; set; }
    public required List<string> Lines { get; set; }
    public required PublicityStatus Publicity { get; set; }

}