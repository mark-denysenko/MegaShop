using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Infrastructure;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IPasswordHasher _passwordHasher;
        private UserContext _userDb;

        public UserController(IPasswordHasher passwordHasher, UserContext context)
        {
            _passwordHasher = passwordHasher;
            _userDb = context;
        }

        // GET api/user 
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return _userDb.Users.ToList();
        }

        // GET api/user/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            return _userDb.Users.FirstOrDefault(u => u.Id == id);
        }

        // POST api/user/authentication
        [HttpPost]
        [Route("authentication")]
        public ActionResult<User> Post([FromBody] UserAuthentication inputUser)
        {
            string passwordHash = _passwordHasher.GetHash(inputUser.Password);

            return _userDb.Users.FirstOrDefault(u => u.Login == inputUser.Login && u.PasswordHash == passwordHash);
        }

        // POST api/user/register
        [HttpPost]
        [Route("register")]
        public ActionResult<User> Post([FromBody] UserRegister inputUser)
        {
            User userFromDb = _userDb.Users.FirstOrDefault(u => u.Login == inputUser.Login);

            if (userFromDb == null)
            {
                string passwordHash = _passwordHasher.GetHash(inputUser.Password);
                User user = new User { Login = inputUser.Login, Name = inputUser.Name, PasswordHash = passwordHash };
                _userDb.Users.Add(user);
                _userDb.SaveChanges();

                return user;
            }
            return StatusCode(StatusCodes.Status409Conflict);
        }

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            User userToDelete = _userDb.Users.FirstOrDefault(u => u.Id == id);

            if (userToDelete != null)
            {
                _userDb.Users.Remove(userToDelete);
                _userDb.SaveChanges();

                return Ok();
            }
            return StatusCode(StatusCodes.Status422UnprocessableEntity);
        }

    }
}