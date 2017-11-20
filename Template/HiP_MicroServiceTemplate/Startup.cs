using HiP_MicroServiceTemplate.Core;
using HiP_MicroServiceTemplate.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSwag;
using NSwag.AspNetCore;
using NSwag.SwaggerGeneration.Processors.Security;
using PaderbornUniversity.SILab.Hip.EventSourcing;
using PaderbornUniversity.SILab.Hip.EventSourcing.EventStoreLlp;
using System.Reflection;

namespace HiP_MicroServiceTemplate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Read configuration from JSON and/or environment variables
            // (see https://docs.microsoft.com/aspnet/core/fundamentals/configuration#use-options-and-configuration-objects)
            services
                .Configure<AuthConfig>(Configuration.GetSection("Auth"))
                .Configure<EndpointConfig>(Configuration.GetSection("Endpoints"))
                .Configure<EventStoreConfig>(Configuration.GetSection("EventStore"));

            // Register services that can be injected into controllers and other services
            // (see https://docs.microsoft.com/aspnet/core/fundamentals/dependency-injection#registering-your-own-services)
            services
                .AddSingleton<EventStoreService>()
                .AddSingleton<CacheDatabaseManager>()
                .AddSingleton<InMemoryCache>()
                .AddSingleton<IDomainIndex, EntityIndex>();

            var serviceProvider = services.BuildServiceProvider();
            var authConfig = serviceProvider.GetService<IOptions<AuthConfig>>();

            // Configure authentication
            services
                .AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Audience = authConfig.Value.Audience;
                    options.Authority = authConfig.Value.Authority;
                });

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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseAuthentication();
            app.UseMvc();

            app.UseSwaggerUi(typeof(Startup).Assembly, new SwaggerUiSettings
            {
                Title = Assembly.GetEntryAssembly().GetName().Name,
                DocExpansion = "list",
                PostProcess = doc =>
                {
                    foreach (var op in doc.Operations)
                    {
                        op.Operation.Parameters.Add(new SwaggerParameter
                        {
                            Name = "Authorization",
                            Kind = SwaggerParameterKind.Header,
                            IsRequired = true
                        });
                    }
                }
            });
        }
    }
}
