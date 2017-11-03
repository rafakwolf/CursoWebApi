using Aula02Api.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Aula02Api.Auth
{
    public class AuthenticationHandler : JwtBearerEvents
    {
        public override Task TokenValidated(TokenValidatedContext context)
        {
            var email = context.Ticket.Principal.Identity.Name;

            var dbContext = context.HttpContext.RequestServices.GetService(typeof(DataContext)) as DataContext;

            if (dbContext != null && !dbContext.Users.Any(u => u.Email == email))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.SkipToNextMiddleware();
            }

            return Task.FromResult(0);
        }
    }
}
