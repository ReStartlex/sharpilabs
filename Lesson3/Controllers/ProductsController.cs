using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using DatabaseAPI.Repositories;
using DatabaseAPI.Repositories.Product;
using DatabaseAPI.Repositories.User;
using DatabaseAPI.Models;
using System.Security.Cryptography.X509Certificates;
using DatabaseAPI;
using Lesson3.Contracts.Product;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;

namespace Lesson3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (_productRepository.TryGet(id, out DBProduct product) == true)
            {
                return Ok(product);
            }

            return BadRequest($"Not found product with id = {id}");
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int? pageLength,
            [FromQuery] int? pageIndex,
            [FromQuery] int? orderType,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            if (pageLength != null && pageIndex != null && orderType != null && minPrice != null && maxPrice != null)
            {
                return Ok(await _productRepository.Get(pageLength.Value, pageIndex.Value, minPrice.Value, maxPrice.Value, (SortType)orderType.Value));
            }

            return Ok(await _productRepository.Get());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AddProductContract addProductContract)
        {
            DBProduct product = new DBProduct();
            product.Name = addProductContract.Name;
            product.Description = addProductContract.Description;
            product.Price = addProductContract.Price;
            product.IsDeleted = false;

            await _productRepository.Add(product);

            return Ok();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (await _productRepository.Remove(id) == false)
                return BadRequest($"Not found product with id = {id}");

            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateProductContract updateProductContract)
        {
            DBProduct product = new DBProduct();
            product.Name = updateProductContract.Name;
            product.Description = updateProductContract.Description;
            product.Price = updateProductContract.Price;

            if (await _productRepository.Update(updateProductContract.Id, product) == true)
            {
                product.Name = updateProductContract.Name;
                product.Description = updateProductContract.Description;
                product.Price = updateProductContract.Price;

                return Ok();
            }

            return BadRequest($"Not found product with id = {updateProductContract.Id}");
        }
    }
}