using System.Threading.Tasks;
using ConfigurationReader.Abstraction;
using ConfigurationReader.Storages.MongoDb.Entities;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace ConfigurationReader.Tests {
    public class ConfigurationReaderTests {
        [Fact]
        public void GetValue_Should_Return_Default_Object_Value_When_Cannot_Cast_Value_To_Requested_Value() {
            // Arrange
            var storageProviderMock = new Mock<IStorageProvider<ObjectId>>();
            var cacheService = new CacheService(new MemoryCache(new MemoryCacheOptions()));

            var options = new ConfigurationReaderOptions {
                ApplicationName = "application-name",
                ConnectionString = string.Empty,
                RefreshTimerIntervalInMs = 1_000
            };

            storageProviderMock
                .Setup(x => x.Get("application-name", "key"))
                .Returns(() => {
                    var configuration = new Configuration {
                        ApplicationName = "application-name",
                        Name = "key",
                        Value = "foobar",
                        IsActive = true
                    };

                    return Task.FromResult((IConfiguration<ObjectId>)configuration);
                });

            var configurationReader = new ConfigurationReader(
                storageProviderMock.Object, cacheService, options);

            // Act
            var value = configurationReader.GetValue<bool>("key");

            // Assert
            Assert.Equal(default(bool), value);
        }

        [Fact]
        public void GetValue_Should_Return_Default_Object_Value_When_IsActive_False() {
            // Arrange
            var storageProviderMock = new Mock<IStorageProvider<ObjectId>>();
            var cacheService = new CacheService(new MemoryCache(new MemoryCacheOptions()));

            var options = new ConfigurationReaderOptions {
                ApplicationName = "application-name",
                ConnectionString = string.Empty,
                RefreshTimerIntervalInMs = 1_000
            };

            storageProviderMock
                .Setup(x => x.Get("application-name", "key"))
                .Returns(() => {
                    var configuration = new Configuration {
                        ApplicationName = "application-name",
                        Name = "key",
                        Value = "value",
                        IsActive = false
                    };

                    return Task.FromResult((IConfiguration<ObjectId>)configuration);
                });

            var configurationReader = new ConfigurationReader(
                storageProviderMock.Object, cacheService, options);

            // Act
            var value = configurationReader.GetValue<string>("key");

            // Assert
            Assert.Equal(default(string), value);
        }

        [Fact]
        public void GetValue_Should_Return_Default_Object_Value_When_Key_Not_Found()
        {
            // Arrange
            var storageProviderMock = new Mock<IStorageProvider<ObjectId>>();
            var cacheService = new CacheService(new MemoryCache(new MemoryCacheOptions()));

            var options = new ConfigurationReaderOptions {
                ApplicationName = "application-name",
                ConnectionString = string.Empty,
                RefreshTimerIntervalInMs = 1_000
            };

            storageProviderMock
                .Setup(x => x.Get("application-name", "key"))
                .Returns(() => {
                    var configuration = new Configuration {
                        ApplicationName = "application-name",
                        Name = "key",
                        Value = "value",
                        IsActive = false
                    };

                    return Task.FromResult((IConfiguration<ObjectId>)configuration);
                });

            var configurationReader = new ConfigurationReader(
                storageProviderMock.Object, cacheService, options);

            // Act
            var value = configurationReader.GetValue<string>("another-key");

            // Assert
            Assert.Equal(default(string), value);
        }

        [Fact]
        public void GetValue_Should_Return_Expected_Value() {
            // Arrange
            var storageProviderMock = new Mock<IStorageProvider<ObjectId>>();
            var cacheService = new CacheService(new MemoryCache(new MemoryCacheOptions()));

            var options = new ConfigurationReaderOptions {
                ApplicationName = "application-name",
                ConnectionString = string.Empty,
                RefreshTimerIntervalInMs = 1_000
            };

            storageProviderMock
                .Setup(x => x.Get("application-name", "key"))
                .Returns(() => {
                    var configuration = new Configuration {
                        ApplicationName = "application-name",
                        Name = "key",
                        Value = "value",
                        IsActive = true
                    };

                    return Task.FromResult((IConfiguration<ObjectId>)configuration);
                });

            var configurationReader = new ConfigurationReader(
                storageProviderMock.Object, cacheService, options);

            // Act
            var value = configurationReader.GetValue<string>("key");

            // Assert
            Assert.Equal("value", value);
        }

        [Fact]
        public void GetValue_Should_Return_Null_When_Application_Name_Not_Matched() {
            // Arrange
            var storageProviderMock = new Mock<IStorageProvider<ObjectId>>();
            var cacheService = new CacheService(new MemoryCache(new MemoryCacheOptions()));

            var options = new ConfigurationReaderOptions {
                ApplicationName = "another-application-name",
                ConnectionString = string.Empty,
                RefreshTimerIntervalInMs = 1_000
            };

            storageProviderMock
                .Setup(x => x.Get("application-name", "key"))
                .Returns(() => {
                    var configuration = new Configuration {
                        ApplicationName = "application-name",
                        Name = "key",
                        Value = "value",
                        IsActive = true
                    };

                    return Task.FromResult((IConfiguration<ObjectId>)configuration);
                });

            var configurationReader = new ConfigurationReader(
                storageProviderMock.Object, cacheService, options);

            // Act
            var value = configurationReader.GetValue<string>("key");

            // Assert
            Assert.Equal(default(string), value);
        }
    }
}