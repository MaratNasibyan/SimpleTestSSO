using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Authentication.Server.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Authentication.Server
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddIdentityServer()
                   .AddDeveloperSigningCredential()
                   .AddInMemoryIdentityResources(Config.GetIdentityResources())
                   .AddInMemoryApiResources(Config.GetApiResources())
                   .AddInMemoryClients(Config.GetClients());
                  // .AddTestUsers(Config.GetUsers());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseIdentityServer();

            app.UseHttpsRedirection();
            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Authentication Server");
            });
        }
    }
}
