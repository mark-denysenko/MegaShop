using System;
using System.Security.Cryptography;
using System.Text;

namespace UserService.Infrastructure
{
    public class PasswordHasher : IPasswordHasher
    {
        public string GetHash(string value)
        {
            return GetHash(Encoding.UTF8.GetBytes(value));
        }

        public string GetHash(byte[] value)
        {
            string hashResult = string.Empty;
            using (SHA256 MySHA256 = SHA256.Create())
            {
                byte[] byteHash = MySHA256.ComputeHash(value);
                hashResult = Convert.ToBase64String(byteHash);
            }

            return hashResult;
        }
    }
}
