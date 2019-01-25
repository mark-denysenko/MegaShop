using System;

namespace UserService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }

        public string RefreshToken { get; set; }
    }
}
