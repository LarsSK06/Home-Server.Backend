using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APIKvihaugenEngine.Models;

public class Loan{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name"), BsonRepresentation(BsonType.String)]
    public required string Name { get; set; }

    [BsonElement("subject"), BsonRepresentation(BsonType.String)]
    public required string Password { get; set; }

    [BsonElement("email"), BsonRepresentation(BsonType.String)]
    public required string Email { get; set; }

    [BsonElement("admin"), BsonRepresentation(BsonType.Boolean)]
    public required bool Admin { get; set; }

}