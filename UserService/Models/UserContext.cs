using System;
using Microsoft.EntityFrameworkCore;

namespace UserService.Models
{
    public class UserContext : DbContext
    {
        private string connectionString = "Server=userserver;Database=userdb;User=sa;Password=Markdev2019;";

        public UserContext()
        {
            // uncomment it when first time SOLUTION running (or userserver - userdb)
            // when db created, comment it and uncomment Migration
            //Database.EnsureCreated();

            //Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<User> Users { get; set; }
        //public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
