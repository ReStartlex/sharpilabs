using Microsoft.AspNetCore.Mvc;
using DatabaseAPI.Repositories.Product;
using DatabaseAPI.Models;
using DatabaseAPI;
using DatabaseAPI.Repositories.Purchase;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Lesson3.Contracts.UserPurchase;

namespace Lesson3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasesController : Controller
    {
        private IUserPurchaseRepository _userPurchaseRepository;

        public PurchasesController(IUserPurchaseRepository userPurchaseRepository)
        {
            _userPurchaseRepository = userPurchaseRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddUserPurchaseContract contract)
        {
            DBUserPurchase dBUserPurchase = new DBUserPurchase();
            dBUserPurchase.ProductId = contract.ProductId;
            dBUserPurchase.UserId = contract.UserId;
            dBUserPurchase.Price = contract.Price;

            await _userPurchaseRepository.Add(dBUserPurchase);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ICollection<DBUserPurchase> purchases = await _userPurchaseRepository.Get();
            return Ok(purchases);
        }

        [HttpGet("UserPurchases")]
        public async Task<IActionResult> Get([FromQuery] int userId)
        {
            ICollection<DBUserPurchase> purchases = await _userPurchaseRepository.Get(userId);
            return Ok(purchases);
        }
    }
}