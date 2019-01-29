using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UserService.Infrastructure
{
    public class TokenService
    {
        public static string GenerateToken(string uniqueName)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, uniqueName)
            };

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: JwtOptions.ISSUER,
                    audience: JwtOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims,
                    expires: now.Add(TimeSpan.FromMinutes(JwtOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(JwtOptions.sharedSymmetricSecurityKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static JwtSecurityToken DecodeToken(string jwtToken)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
            //jwt.SigningKey = JwtOptions.sharedSymmetricSecurityKey;

            return jwt;
        }

        public static bool IsRefreshTokenExpired(string refreshToken)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
            return jwt.ValidTo < DateTime.Now;
        }

        public static string RefreshIdentifierPart(string refreshToken)
        {
            IPasswordHasher hasher = new PasswordHasher();
            refreshToken = refreshToken.Trim();

            return hasher.GetHash(refreshToken);
        }
    }
}
