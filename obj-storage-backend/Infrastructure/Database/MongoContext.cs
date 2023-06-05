using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Database
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;
        private readonly MongoClient _client;

        public MongoContext(IOptions<MongodbSettings> settings)
        {
            _client = new MongoClient(settings.Value.ConnectionString);
            _database = _client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            if (!CollectionExists(_database, collectionName))
            {
                _database.CreateCollection(collectionName);
            }
            return _database.GetCollection<T>(collectionName);
        }

        private static bool CollectionExists(IMongoDatabase database, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var options = new ListCollectionNamesOptions { Filter = filter };
            return database.ListCollectionNames(options).Any();
        }
    }
}
