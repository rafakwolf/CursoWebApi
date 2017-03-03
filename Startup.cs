using Aula02Api.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Aula02Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // ============== middleware condicional 01 ====================== //

        private static void HandleBranch(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Condição encontrada!");
            });
        }

        // ===================== middleware condicional 02 ================= //

        private static void HandleMapTest(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("middleware por mapeamento encontrado!");
            });
        }

        public void ConfigureMapping(IApplicationBuilder app)
        {
            app.Map("/maptest", HandleMapTest);
        }

        // =========================== //

        public void ConfigureMapWhen(IApplicationBuilder app)
        {
            app.MapWhen(context =>
            {
                return context.Request.Query.ContainsKey("executar");
            }, HandleBranch);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // app.Run(async context =>
            // {
            //     await context.Response.WriteAsync("Aioooo silverrrr!");
            // }); 

            // app.Use(async (context, next) =>
            // {
            //     //action before next delegate
            //     await next.Invoke(); //call next middleware
            //     //action after called middleware
            // });

            // this.ConfigureMapWhen(app);      

             this.ConfigureMapping(app);                

        //   app.ApplyUserValidation();


            app.UseMvc();

           // app.UseWelcomePage();
        }
    }
}
