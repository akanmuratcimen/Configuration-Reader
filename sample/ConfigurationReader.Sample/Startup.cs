using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigurationReader.Sample {
    public class Startup {
        public Startup() {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", false, true)
                .Build();
        }

        private IConfigurationRoot Configuration { get; }

        public static void Main() {
            WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build().Run();
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddConfigurationReader(options => {
                options.ApplicationName = Configuration.GetValue<string>("ApplicationName");
                options.ConnectionString = Configuration.GetValue<string>("ConnectionString");
                options.RefreshTimerIntervalInMs = Configuration.GetValue<int>("RefreshTimerIntervalInMs");
            });
        }

        public void Configure(IApplicationBuilder app) {
            app.UseDeveloperExceptionPage();

            app.Run(async context => {
                var configurationReader = context.RequestServices.GetService<IConfigurationReader>();
                var value = configurationReader.GetValue<string>("SiteName");

                await context.Response.WriteAsync(value ?? string.Empty);
            });
        }
    }
}