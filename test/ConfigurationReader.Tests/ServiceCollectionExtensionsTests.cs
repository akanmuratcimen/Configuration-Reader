using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ConfigurationReader.Tests {
    public class ServiceCollectionExtensionsTests {
        [Fact]
        public void AddConfigurationReader_Should_Throw_ArgumentException_When_ApplicationName_Null() {
            var serviceCollection = new ServiceCollection();

            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => {
                serviceCollection.AddConfigurationReader(options => {
                    options.ApplicationName = null;
                    options.ConnectionString = "connection-string";
                    options.RefreshTimerIntervalInMs = 1000;
                });
            });
        }

        [Fact]
        public void AddConfigurationReader_Should_Throw_ArgumentException_When_ConnectionString_Null() {
            var serviceCollection = new ServiceCollection();

            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => {
                serviceCollection.AddConfigurationReader(options => {
                    options.ApplicationName = "applicaton-name";
                    options.ConnectionString = null;
                    options.RefreshTimerIntervalInMs = 1000;
                });
            });
        }

        [Fact]
        public void AddConfigurationReader_Should_Throw_ArgumentOutOfRangeException_When_RefreshTimerIntervalInMs_Equals_To_Zero() {
            var serviceCollection = new ServiceCollection();

            // Arrange & Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                serviceCollection.AddConfigurationReader(options => {
                    options.ApplicationName = "applicaton-name";
                    options.ConnectionString = "connection-string";
                    options.RefreshTimerIntervalInMs = 0;
                });
            });
        }
    }
}