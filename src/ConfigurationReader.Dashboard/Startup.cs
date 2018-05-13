using System.IO;
using ConfigurationReader.Storages.MongoDb;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigurationReader.Dashboard {
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
            services.AddMongoDbStorageProvider(Configuration.GetValue<string>("ConnectionString"));
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app) {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}