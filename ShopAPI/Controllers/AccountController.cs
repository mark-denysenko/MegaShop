using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopAPI.HttpClients;
using ShopAPI.Infrastructure;
using ShopAPI.Models;

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
        public async Task<IActionResult> Token([FromForm]string login, [FromForm]string password)
        {
            User identity = await _userClient.GetUserByLoginAndPassword(login, password);

            if (identity != null)
                return new ObjectResult(new { userid = identity.Id, username = identity.Name, access_token = TokenService.GenerateToken(login) });

            return BadRequest("Invalid login or password");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] string login, [FromForm] string name, [FromForm] string password)
        {
            User newUser = await _userClient.RegisterUser(login, name, password);

            if (newUser != null)
                return Ok("User created!");

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
