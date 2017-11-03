using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Aula02Api.Auth
{
    public class JwtProvider
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly JwtSettings _settings;

        public JwtProvider(JwtSettings settings, JwtSecurityTokenHandler tokenHandler)
        {
            _settings = settings;
            _tokenHandler = tokenHandler;
        }

        public string CreateEncoded(string userName)
        {
            return _tokenHandler.CreateEncodedJwt(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new GenericIdentity(userName)),
                Expires = DateTime.UtcNow.AddDays(_settings.TokenExpiration),
                SigningCredentials = new SigningCredentials(_settings.SecurityKey, SecurityAlgorithms.HmacSha256Signature),
                Audience = _settings.Audience,
                Issuer = _settings.Issuer
            });
        }
    }
}
