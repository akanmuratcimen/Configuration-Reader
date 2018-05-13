using System;
using ConfigurationReader.Storages.MongoDb;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigurationReader {
    public static class ServiceCollectionExtensions {
        public static void AddConfigurationReader(this IServiceCollection serviceCollection,
            Action<ConfigurationReaderOptions> options) {
            serviceCollection.AddMemoryCache();

            var configurationReaderOptions = new ConfigurationReaderOptions();

            options(configurationReaderOptions);

            ValidateOptions(configurationReaderOptions);

            serviceCollection.AddSingleton(configurationReaderOptions);
            serviceCollection.AddMongoDbStorageProvider(configurationReaderOptions.ConnectionString);
            serviceCollection.AddTransient<ICacheService, CacheService>();
            serviceCollection.AddTransient<IConfigurationReader, ConfigurationReader>();
        }

        private static void ValidateOptions(ConfigurationReaderOptions options) {
            if (string.IsNullOrEmpty(options.ApplicationName)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(options.ApplicationName));
            }

            if (string.IsNullOrEmpty(options.ConnectionString)) {
                throw new ArgumentException("Value cannot be null or empty.", nameof(options.ConnectionString));
            }

            if (options.RefreshTimerIntervalInMs <= 0) {
                throw new ArgumentOutOfRangeException(nameof(options.RefreshTimerIntervalInMs));
            }
        }
    }
}