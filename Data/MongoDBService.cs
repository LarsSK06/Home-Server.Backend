using MongoDB.Driver;

namespace APIKvihaugenEngine.Data{
    public class MongoDBService{

        private readonly IConfiguration _config;
        private readonly IMongoDatabase? _database;

        public MongoDBService(IConfiguration config){
            _config = config;

            MongoUrl url = MongoUrl.Create(_config.GetConnectionString("DatabaseConnection"));
            MongoClient client = new (url);
            
            _database = client.GetDatabase(url.DatabaseName);
        }

        public IMongoDatabase? Database => _database;

    }
}