using ShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShopAPI.HttpClients
{
    public class UserServiceClient: IUserServiceClient
    {
        private HttpClient _client;

        public UserServiceClient()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            _client = new HttpClient(handler);
        }

        public async Task<bool> DeleteUser(int id)
        {
            var response = await _client.DeleteAsync(SERVICES_URLS.UserService.DeleteUser + id);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<User> GetUserById(int id)
        {
            var response = await _client.GetAsync(SERVICES_URLS.UserService.DeleteUser + id);

            return await response.Content.ReadAsAsync<User>();
        }

        public async Task<User> GetUserByLoginAndPassword(string login, string password)
        {
            var response = await _client.PostAsJsonAsync(SERVICES_URLS.UserService.Authentication, new { login, password });
            return await response.Content.ReadAsAsync<User>();
        }

        public async Task<User> RegisterUser(string login, string name, string password)
        {
            var response = await _client.PostAsJsonAsync(SERVICES_URLS.UserService.Register, new { login, name, password });
            return await response.Content.ReadAsAsync<User>();
        }
    }
}
