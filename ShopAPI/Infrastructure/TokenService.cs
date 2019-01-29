using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ShopAPI.HttpClients;

namespace ShopAPI.Infrastructure
{
    public class TokenService
    {
        public static async Task<string> GenerateToken(string uniqueName)
        {
            string refreshIdentifier = await new UserServiceClient().GetRefreshIdentifierByLogin(uniqueName);
            return GenerateToken(uniqueName, refreshIdentifier);
        }

        public static string GenerateToken(string uniqueName, string refreshIdentifier)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, uniqueName),
                new Claim(JwtRegisteredClaimNames.Jti, refreshIdentifier)
            };

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.sharedSymmetricSecurityKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static JwtSecurityToken DecodeToken(string jwtToken)
        {
            // need check security key 
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken.Trim());

            return jwt;
        }

        public static string GetLoginFromToken(string jwtToken)
        {
            var jwt = DecodeToken(jwtToken);

            return jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value;
        }

        public static bool IsExpiredToken(string token)
        {
            var jwt = DecodeToken(token);

            return jwt.ValidTo < DateTime.Now;
        }

        public static async Task<string> TryRefreshAccesTokenAsync(string accesToken)
        {
            var jwt = DecodeToken(accesToken);

            string login = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value;
            string jwtRefreshIdentidier = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            string refreshIdentifier = await new UserServiceClient().GetRefreshIdentifierByLogin(login);

            if (jwtRefreshIdentidier == refreshIdentifier)
                return GenerateToken(login, refreshIdentifier);
            else
                return null;
        }
    }
}
