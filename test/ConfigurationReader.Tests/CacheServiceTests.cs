using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace ConfigurationReader.Tests {
    public class CacheServiceTests {
        [Fact]
        public async Task GetValue_Should_Return_Expected_Value() {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheService = new CacheService(memoryCache);

            var expirationTime = TimeSpan.FromMilliseconds(1000);

            var value = await cacheService.GetValue("an-applicaton-name", "a-key",
                expirationTime, (applicatonName, key) => Task.FromResult("value"));

            Assert.Equal("value", value);
        }

        [Fact]
        public async Task GetValue_Should_Return_New_Value_When_Cache_Expired() {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheService = new CacheService(memoryCache);

            var expirationTime = TimeSpan.FromMilliseconds(10);

            await cacheService.GetValue("an-applicaton-name", "a-key",
                expirationTime, (applicatonName, key) => Task.FromResult("value"));

            Thread.Sleep(20);

            var value = await cacheService.GetValue("an-applicaton-name", "a-key",
                expirationTime, (applicatonName, key) => Task.FromResult("new-value"));

            Assert.Equal("new-value", value);
        }

        [Fact]
        public async Task GetValue_Should_Return_Previous_Value_When_Current_Value_Throws_An_Exception() {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheService = new CacheService(memoryCache);

            var expirationTime = TimeSpan.FromMilliseconds(10);

            await cacheService.GetValue("an-applicaton-name", "a-key",
                expirationTime, (applicatonName, key) => Task.FromResult("value"));

            Thread.Sleep(20);

            var value = await cacheService.GetValue<string>("an-applicaton-name", "a-key",
                expirationTime, (applicatonName, key) => throw new Exception());

            Assert.Equal("value", value);
        }

        [Fact]
        public async Task GetValue_Should_Return_Same_Value_Before_Cache_Expired() {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheService = new CacheService(memoryCache);

            var expirationTime = TimeSpan.FromMilliseconds(10);

            await cacheService.GetValue("an-applicaton-name", "a-key",
                expirationTime, (applicatonName, key) => Task.FromResult("value"));

            Thread.Sleep(5);

            var value = await cacheService.GetValue("an-applicaton-name", "a-key",
                expirationTime, (applicatonName, key) => Task.FromResult("new-value"));

            Assert.Equal("value", value);
        }
    }
}