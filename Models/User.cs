using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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

    public PublicUser ToPublic(){
        return new PublicUser{
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

public class PublicUser{

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