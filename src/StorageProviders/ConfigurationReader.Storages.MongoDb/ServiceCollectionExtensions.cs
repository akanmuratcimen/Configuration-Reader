using ConfigurationReader.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace ConfigurationReader.Storages.MongoDb {
    public static class ServiceCollectionExtensions {
        public static void AddMongoDbStorageProvider(this IServiceCollection serviceCollection, string connectionString) {
            var mongoConnectionString = new ConnectionString(connectionString);
            var collectionName = mongoConnectionString.GetOption("Collection");

            serviceCollection.Add(new ServiceDescriptor(typeof(IMongoDatabase), provider => {
                var client = new MongoClient(connectionString);

                return client.GetDatabase(mongoConnectionString.DatabaseName);
            }, ServiceLifetime.Singleton));

            serviceCollection.Add(new ServiceDescriptor(typeof(CollectionOptions),
                provider => new CollectionOptions { Name = collectionName },
                ServiceLifetime.Singleton));

            serviceCollection.AddTransient(typeof(IStorageProvider<ObjectId>), typeof(MongoDbStorageProvider));
        }
    }
}