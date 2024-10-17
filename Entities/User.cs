using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APIKvihaugenEngine.Entities{
    public class User{

        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public required string Name { get; set; }

        [BsonElement("password"), BsonRepresentation(BsonType.String)]
        public required string Password { get; set; }

        [BsonElement("email"), BsonRepresentation(BsonType.String)]
        public required string Email { get; set; }

    }
}