using DatabaseAPI.Models;
using DatabaseAPI.Repositories.User;
using Lesson3.Authorization;
using Lesson3.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Lesson3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AuthorizationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AuthorizeUserContract contract)
        {
            string passwordHash = AuthOptions.GetPassawordHash(contract.Password);
            DBUser dbUser = _userRepository.Get().Result.FirstOrDefault(x =>
                x.Login.ToLower() == contract.Login.ToLower() && x.Password == passwordHash);

            ClaimsIdentity identity = GetIdentity(contract.Login);
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: DateTime.Now,
                claims: identity.Claims,
                expires: DateTime.Now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(
                    AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            GetAuthorizeContract authorizeContract = new GetAuthorizeContract();
            authorizeContract.Token = encodedJwt;
            authorizeContract.Lifetime = AuthOptions.LIFETIME;

            return Ok(authorizeContract);
        }

        private ClaimsIdentity GetIdentity(string login)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}