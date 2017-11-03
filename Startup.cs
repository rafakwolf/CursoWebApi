using Aula02Api.Auth;
using Aula02Api.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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

            /* JWT */

            var key = Encoding.UTF8.GetBytes(Configuration["Auth:SecurityKey"]);
            var expiration = int.Parse(Configuration["Auth:Expiration"]);

            services.AddSingleton(new JwtSettings(new SymmetricSecurityKey(key), expiration));
            services.AddSingleton<AuthenticationHandler>();
            services.AddTransient<JwtSecurityTokenHandler>();
            services.AddTransient<JwtProvider>();

            /* JWT */

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Minha API", Version = "v1" });
            });

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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, AuthenticationHandler handler, JwtSettings jwtSettings)
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Curso Web Api");
            });


            /* JWT */

            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = jwtSettings.SecurityKey,
                ValidAudience = jwtSettings.Audience,
                ValidIssuer = jwtSettings.Issuer
            };

            var options = new JwtBearerOptions
            {
                Events = handler,
                TokenValidationParameters = validationParameters
            };

            app.UseJwtBearerAuthentication(options);

            /* JWT */


            app.UseMvc();
        }
    }
}
