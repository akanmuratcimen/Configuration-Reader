using System;
using System.Globalization;
using System.Threading.Tasks;
using ConfigurationReader.Abstraction;
using MongoDB.Bson;

namespace ConfigurationReader {
    public interface IConfigurationReader {
        T GetValue<T>(string key);
    }

    public class ConfigurationReader : IConfigurationReader {
        private readonly ICacheService _cacheService;
        private readonly ConfigurationReaderOptions _options;
        private readonly IStorageProvider<ObjectId> _provider;

        public ConfigurationReader(
            IStorageProvider<ObjectId> provider,
            ICacheService cacheService,
            ConfigurationReaderOptions options) {
            _provider = provider;
            _cacheService = cacheService;
            _options = options;
        }

        private TimeSpan ExpirationTime {
            get { return TimeSpan.FromMilliseconds(_options.RefreshTimerIntervalInMs); }
        }

        public T GetValue<T>(string key) {
            return GetValueAsync<T>(key).GetAwaiter().GetResult();
        }

        private async Task<T> GetValueAsync<T>(string key) {
            return await _cacheService.GetValue(
                _options.ApplicationName, key, ExpirationTime, GetValue<T>);
        }

        private async Task<T> GetValue<T>(string applicatonName, string name) {
            var configuration = await _provider.Get(applicatonName, name);

            if (configuration == null || !configuration.IsActive) {
                return default(T);
            }

            try {
                return (T)Convert.ChangeType(configuration.Value,
                    typeof(T), CultureInfo.InvariantCulture);
            } catch {
                return default(T);
            }
        }
    }
}