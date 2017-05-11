using Aula02Api.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer().AddDbContext<DataContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("BancoDados")));


            services.AddMvc()
                    .AddJsonOptions(options =>
                      options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // *** Exemplo Middleware *** // 
                // app.Run(async context =>
                // {
                //     await context.Response.WriteAsync("Aioooo silverrrr!");
                // }); 

            // *** Exemplo Middleware *** //
                // app.Use(async (context, next) =>
                // {
                //     //action before next delegate
                //     await next.Invoke(); //call next middleware
                //     //action after called middleware
                // });

            // *** Exemplo Middleware *** //
                // this.ConfigureMapWhen(app);      

            this.ConfigureMapping(app);

            // *** Exemplo Middleware *** //
                // app.ApplyUserValidation();

            app.UseMvc();
        }
    }
}
