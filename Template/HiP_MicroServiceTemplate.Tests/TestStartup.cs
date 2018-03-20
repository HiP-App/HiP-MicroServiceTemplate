using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PaderbornUniversity.SILab.Hip.EventSourcing;
using PaderbornUniversity.SILab.Hip.EventSourcing.EventStoreLlp;
using PaderbornUniversity.SILab.Hip.EventSourcing.FakeStore;
using PaderbornUniversity.SILab.Hip.EventSourcing.Mongo;
using PaderbornUniversity.SILab.Hip.EventSourcing.Mongo.Test;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Core;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Utility;
using PaderbornUniversity.SILab.Hip.Webservice;
using System.Collections.Generic;

namespace PaderbornUniversity.SILab.Hip.HiP._MicroServiceTemplate.Tests
{
    public class TestStartup
    {
        public TestStartup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "EventStore:Host", "" },
                    { "EventStore:Stream", "test" }
                })
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            //Initialize resource types
            ResourceTypes.Initialize();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Read configuration from JSON and/or environment variables
            // (see https://docs.microsoft.com/aspnet/core/fundamentals/configuration#use-options-and-configuration-objects)
            services
                .Configure<EndpointConfig>(Configuration.GetSection("Endpoints"))
                .Configure<EventStoreConfig>(Configuration.GetSection("EventStore"))
                .Configure<AuthConfig>(Configuration.GetSection("Auth"))
                .Configure<CorsConfig>(Configuration);

            // Register services that can be injected into controllers and other services
            // (see https://docs.microsoft.com/aspnet/core/fundamentals/dependency-injection#registering-your-own-services)
            services
                .AddSingleton<IEventStore, FakeEventStore>()
                .AddSingleton<IMongoDbContext, FakeMongoDbContext>()
                .AddSingleton<EventStoreService>()
                .AddSingleton<CacheDatabaseManager>()
                .AddSingleton<InMemoryCache>()
                .AddSingleton<IDomainIndex, EntityIndex>();

            var serviceProvider = services.BuildServiceProvider();
            var authConfig = serviceProvider.GetService<IOptions<AuthConfig>>();

            // Configure authentication
            services
                .AddAuthentication(FakeAuthentication.AuthenticationScheme)
                .AddFakeAuthenticationScheme();

            // Configure authorization
            var domain = authConfig.Value.Authority;
            services.AddAuthorization(options =>
            {
                // TODO: Add required scopes
                // options.AddPolicy("read:myservice",
                //     policy => policy.Requirements.Add(new HasScopeRequirement("read:myservice", domain)));
            });

            services.AddCors();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<CorsConfig> corsConfig)
        {

            // CacheDatabaseManager should start up immediately (not only when injected into a controller or
            // something), so we manually request an instance here
            app.ApplicationServices.GetService<CacheDatabaseManager>();

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}