using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopAPI.HttpClients;
using ShopAPI.Infrastructure;
using ShopAPI.Models.UserModels;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserServiceClient _userClient;

        public AccountController()
        {
            _userClient = new UserServiceClient();
        }

        [HttpGet("check")]
        [Authorize]
        public string Check()
        {
            return "Success!";
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody] UserLogin user)
        {
            if (string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.Password))
                return BadRequest("Input login/password!");

            User identity = await _userClient.AuthenticateUser(user.Login, user.Password);

            if (identity != null)
                return new ObjectResult(new { userid = identity.Id, login = identity.Login, username = identity.Name, access_token = await TokenService.GenerateToken(user.Login) });

            return BadRequest("Invalid login or password");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegister user)
        {
            User newUser = await _userClient.RegisterUser(user.Login, user.Name, user.Password);

            if (newUser != null)
                return Ok(newUser);

            return BadRequest("Can't register user!");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool isUserDeleted = await _userClient.DeleteUser(id);

            if (isUserDeleted)
                return Ok("User was deleted");

            return BadRequest("User wasn't deleted!");
        }
    }
}
