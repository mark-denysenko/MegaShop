using ShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAPI.HttpClients
{
    public interface IUserServiceClient
    {
        Task<User> GetUserByLoginAndPassword(string login, string password);
        Task<User> GetUserById(int id);
        Task<User> RegisterUser(string login, string name, string password);
        Task<bool> DeleteUser(int id);
    }
}
