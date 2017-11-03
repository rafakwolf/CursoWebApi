using Microsoft.IdentityModel.Tokens;

namespace Aula02Api.Auth
{
    public class JwtSettings
    {
        public SymmetricSecurityKey SecurityKey { get; }

        public int TokenExpiration { get; }
        public string Audience { get; } = "DummyAudience"; // Destinatário pretendido
        public string Issuer { get; } = "DummyIssuer"; // Identifica quem emitiu o token

        public JwtSettings(SymmetricSecurityKey securityKey, int tokenExpiration)
        {
            SecurityKey = securityKey;
            TokenExpiration = tokenExpiration;
        }
    }
}
