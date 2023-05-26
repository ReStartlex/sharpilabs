using Microsoft.AspNetCore.Mvc;
using DatabaseAPI.Repositories.Product;
using DatabaseAPI.Models;
using DatabaseAPI;
using DatabaseAPI.Repositories.Purchase;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Lesson3.Contracts.UserPurchase;
using System.Diagnostics.SymbolStore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DatabaseAPI.Repositories.User;

namespace Lesson3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasesController : Controller
    {
        private IUserPurchaseRepository _userPurchaseRepository;
        private IUserRepository _userRepository;
        private IProductRepository _productRepository;

        public PurchasesController(IUserPurchaseRepository userPurchaseRepository, IUserRepository userRepository, IProductRepository productRepository)
        {
            _userPurchaseRepository = userPurchaseRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AddAuthUserPurchaseContract contract)
        {
            string? login = HttpContext.User
                ?.Identities
                ?.Where(identity => identity.Claims != null)
                ?.SelectMany(identity => identity.Claims)
                ?.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType)
                ?.Value;

            DBProduct dbProduct = await _productRepository.Get(contract.ProductId);
            DBUser dbUser = await _userRepository.Get(login.ToLower());

            DBUserPurchase dBUserPurchase = new DBUserPurchase();
            dBUserPurchase.ProductId = contract.ProductId;
            dBUserPurchase.UserId = dbUser.Id;
            dBUserPurchase.Price = dbProduct.Price;

            await _userPurchaseRepository.Add(dBUserPurchase);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            string? login = HttpContext.User
                ?.Identities
                ?.Where(identity => identity.Claims != null)
                ?.SelectMany(identity => identity.Claims)
                ?.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType)
                ?.Value;

            DBUser dbUser = await _userRepository.Get(login.ToLower());
            ICollection<DBUserPurchase> purchases = await _userPurchaseRepository.Get(dbUser.Id);

            return Ok(purchases);
        }

        [HttpGet("UserPurchases")]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] int userId)
        {
            ICollection<DBUserPurchase> purchases = await _userPurchaseRepository.Get(userId);
            return Ok(purchases);
        }
    }
}