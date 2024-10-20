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

}

public class PublicLoan{

    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required bool Admin { get; set; }
    public required IEnumerable<PublicLoanEmbed> Loans { get; set; }

}

public class PublicLoanEmbed{

    public required int Id { get; set; }

}

public class MutableUser{

    public required string Name { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }

}