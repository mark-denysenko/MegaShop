using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopAPI.Models.UserModels;

namespace ShopAPI.HttpClients
{
    public interface IUserServiceClient
    {
        Task<User> AuthenticateUser(string login, string password);
        Task<User> GetUserById(int id);
        Task<User> RegisterUser(string login, string name, string password);
        Task<bool> DeleteUser(int id);
    }
}
