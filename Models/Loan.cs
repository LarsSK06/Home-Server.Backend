using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomeServer.Models;

public class Loan{

    [BsonId]
    [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string? MongoId { get; set; }

    [BsonElement("id"), BsonRepresentation(BsonType.Int32)]
    public required int Id { get; set; }

    [BsonElement("name"), BsonRepresentation(BsonType.String)]
    public required string Name { get; set; }

    [BsonElement("subject"), BsonRepresentation(BsonType.String)]
    public required string Subject { get; set; }

    [BsonElement("item"), BsonRepresentation(BsonType.String)]
    public required string Item { get; set; }

    [BsonElement("object"), BsonRepresentation(BsonType.String)]
    public required string Object { get; set; }

    [BsonElement("ownerId"), BsonRepresentation(BsonType.Int32)]
    public required int OwnerId { get; set; }

    public PublicLoan ToPublic(){
        return new PublicLoan{
            Id = Id,
            Name = Name,
            Subject = Subject,
            Item = Item,
            Object = Object,
            Owner = null
        };
    }

    public MutableLoan ToMutable(){
        return new MutableLoan{
            Name = Name,
            Subject = Subject,
            Item = Item,
            Object = Object,
            OwnerId = OwnerId
        };
    }

}

public class PublicLoan{

    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Subject { get; set; }
    public required string Item { get; set; }
    public required string Object { get; set; }
    public required PublicUser Owner { get; set; }

}

public class MutableLoan{

    public required string Name { get; set; }
    public required string Subject { get; set; }
    public required string Item { get; set; }
    public required string Object { get; set; }
    public required int OwnerId { get; set; }

}