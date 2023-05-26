using DatabaseAPI.Models;
using DatabaseAPI.Repositories.User;
using Lesson3.Authorization;
using Lesson3.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Lesson3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        private readonly IUserRepository _userRepository;

        public RegisterController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterUserContract registerUserContract)
        {
            DBUser dbUser = new DBUser();
            dbUser.Name = registerUserContract.Name;
            dbUser.Login = registerUserContract.Login.ToLower();
            dbUser.Password = AuthOptions.GetPassawordHash(registerUserContract.Password);

            await _userRepository.Add(dbUser);

            return Ok();
        }
    }
}