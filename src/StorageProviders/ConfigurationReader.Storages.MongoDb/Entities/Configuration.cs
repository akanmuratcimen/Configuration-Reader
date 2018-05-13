using ConfigurationReader.Abstraction;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ConfigurationReader.Storages.MongoDb.Entities {
    internal class Configuration : IConfiguration<ObjectId> {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string ApplicationName { get; set; }
        public object Value { get; set; }
    }
}