using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ShopAPI
{
    internal class AuthOptions
    {
        public const string ISSUER = "web_api"; // издатель токена
        public const string AUDIENCE = "users"; // потребитель токена
        public const int LIFETIME = 1;

        const string KEY = "mysupersecret_secretkey!123456789";   // ключ для шифрации

        // For example only! Don't store your shared keys as strings in code.
        // Use environment variables or the .NET Secret Manager instead. Configuration["SigningKey"]
        public static SymmetricSecurityKey sharedSymmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}
