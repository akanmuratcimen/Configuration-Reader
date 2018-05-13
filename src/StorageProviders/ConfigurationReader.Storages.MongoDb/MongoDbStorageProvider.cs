using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigurationReader.Abstraction;
using ConfigurationReader.Storages.MongoDb.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ConfigurationReader.Storages.MongoDb {
    internal class MongoDbStorageProvider : IStorageProvider<ObjectId> {
        private readonly CollectionOptions _collectionOptions;
        private readonly IMongoDatabase _database;

        public MongoDbStorageProvider(
            IMongoDatabase database,
            CollectionOptions collectionOptions) {
            _database = database;
            _collectionOptions = collectionOptions;
        }

        internal IMongoCollection<Configuration> Collection {
            get { return _database.GetCollection<Configuration>(_collectionOptions.Name); }
        }

        public async Task<IEnumerable<IConfiguration<ObjectId>>> Configurations() {
            return await Collection.Find(_ => true).ToListAsync();
        }

        public async Task<IConfiguration<ObjectId>> Get(string applicatonName, string name) {
            return await Collection
                .Find(x => x.ApplicationName == applicatonName && x.Name == name)
                .FirstOrDefaultAsync();
        }

        public async Task<IConfiguration<ObjectId>> Get(ObjectId id) {
            return await Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Add(ConfigurationModel model) {
            var configuration = new Configuration();

            configuration.ApplicationName = model.ApplicationName;
            configuration.Name = model.Name;
            configuration.Value = model.Value;
            configuration.IsActive = model.IsActive;

            await Collection.InsertOneAsync(configuration);
        }

        public async Task<bool> Update(ObjectId id, ConfigurationModel model) {
            var configuration = new Configuration();

            configuration.Id = id;
            configuration.ApplicationName = model.ApplicationName;
            configuration.Name = model.Name;
            configuration.Value = model.Value;
            configuration.IsActive = model.IsActive;

            var result = await Collection.ReplaceOneAsync(x => x.Id == id, configuration);

            return result.IsAcknowledged && result.MatchedCount > 0;
        }
    }
}