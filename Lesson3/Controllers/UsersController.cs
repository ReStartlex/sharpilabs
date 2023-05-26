using DatabaseAPI;
using DatabaseAPI.Models;
using DatabaseAPI.Repositories.User;
using Lesson3.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lesson3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await userRepository.Get();
            return Ok(users);
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> Get([FromQuery] int length = 10, [FromQuery] int index = 0)
        {
            ICollection<DBUser> users = await userRepository.Get(length, index);
            return Ok(users);
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserContract updateUserContract)
        {
            DBUser dBUser = new DBUser()
            {
                Name = updateUserContract.Name,
                Login = updateUserContract.Login,
                Password = updateUserContract.Password
            };

            bool result = await userRepository.Update(updateUserContract.Id, dBUser);

            if(result == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest($"Пользователь с айди {updateUserContract.Id} не обнаружен");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] AddUserContract addUserContract)
        {
            DBUser dBUser = new DBUser()
            {
                Name = addUserContract.Name,
                Login = addUserContract.Login,
                Password = addUserContract.Password
            };

            await userRepository.Add(dBUser); 

            return Ok();
        }
    }
}
