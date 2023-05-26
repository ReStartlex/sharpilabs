using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using labisharp.Contracts;
using labisharp.Data;
using labisharp.Repositories;

namespace labisharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        [HttpGet]
        public IActionResult Get([FromQuery] int? id)
        {
            if (id == null) return Ok(UsersRepository.GetData());
            else
            {
                if (UsersRepository.TryGet(id.Value, out User user) == true)
                    return Ok(user);
                else return BadRequest($"User ID = {id.Value} not found");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddUserContract addUserContract)
        {
            UsersRepository.Add(addUserContract.Name, addUserContract.Login, addUserContract.Password);
            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] EditUserContract editUserContract)
        {
            if (UsersRepository.TryEdit(editUserContract.Id, editUserContract.Name, editUserContract.Login) == true)
                return Ok();
            else return BadRequest($"User ID = {editUserContract.Id} not found");

        }
    }
}
