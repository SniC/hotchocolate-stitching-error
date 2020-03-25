using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.Stitching;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("one", (sp, client) =>
            {
                client.BaseAddress = new Uri("http://localhost:5000");
            });
            services.AddHttpClient("two", (sp, client) =>
            {
                client.BaseAddress = new Uri("http://localhost:5010");
            });

            services.AddHttpContextAccessor();

            services.AddDataLoaderRegistry();
            services.AddGraphQLSubscriptions();
            services.AddStitchedSchema(builder => {
                builder
                .AddSchema("gateway", new SchemaBuilder().AddQueryType<Query>().Create())
                .AddSchemaFromHttp("one")
                .AddSchemaFromHttp("two");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGraphQL();
            app.UsePlayground();
        }
    }
}
