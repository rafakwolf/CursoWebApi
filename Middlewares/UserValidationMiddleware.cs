using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Aula02Api.Middlewares
{
    public class UserValidationMiddleware
    {
        private RequestDelegate _next;

        public UserValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.Keys.Contains("userName"))
            {
                context.Response.StatusCode = 400; //Bad Request                
                await context.Response.WriteAsync("Informações do usuário não encontradas");
                return;
            }
            else
            {
                // Aqui poderia validar no banco de dados por exemplo.

                if (context.Request.Headers["userName"] != "cursowebapi")
                {
                    context.Response.StatusCode = 401; //UnAuthorized
                    await context.Response.WriteAsync("Usuário não autorizado.");
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }

    public static class UserValidationMiddlewareExtension
    {
        public static IApplicationBuilder ApplyUserValidation(this IApplicationBuilder app)
        {
            app.UseMiddleware<UserValidationMiddleware>();
            return app;
        }
    }
}
