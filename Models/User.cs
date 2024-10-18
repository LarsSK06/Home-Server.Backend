using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APIKvihaugenEngine.Models;

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

}