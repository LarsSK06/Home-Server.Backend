using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomeServer.Models;

public class Session{

    [BsonId]
    [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string? MongoId { get; set; }

    [BsonElement("token"), BsonRepresentation(BsonType.Int32)]
    public required int Token { get; set; }

    [BsonElement("userId"), BsonRepresentation(BsonType.Int32)]
    public required int UserId { get; set; }

    public PublicSession ToPublic(){
        return new PublicSession{
            Token = Token,
            User = null
        };
    }

}

public class PublicSession{

    public required int Token { get; set; }
    public required PublicUser User { get; set; }

}