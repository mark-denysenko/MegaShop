using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Infrastructure
{
    internal class JwtOptions
    {
        public const string ISSUER = "userservice"; 
        public const string AUDIENCE = "users";
        public const int LIFETIME = 60 * 24 * 30; // in minutes

        const string KEY = "viP1tg4T7LTKZieLi6taFLuDJGgib7DG";

        public static SymmetricSecurityKey sharedSymmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}
