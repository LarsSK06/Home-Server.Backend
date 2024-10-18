using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APIKvihaugenEngine.Entities;

public class User{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name"), BsonRepresentation(BsonType.String)]
    public required string Name { get; set; }

    [BsonElement("password"), BsonRepresentation(BsonType.String)]
    public required string Password { get; set; }

    [BsonElement("email"), BsonRepresentation(BsonType.String)]
    public required string Email { get; set; }

}