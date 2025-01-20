using App.Core.Dto;
using App.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EComApplicationBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _producrService;
        private readonly IRegisterService _registerService;


        public ProductController(IProductService productService, IRegisterService registerService)
        {
            _producrService = productService;
            _registerService = registerService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("userId")]
        public async Task<IActionResult> getProductsByUser(int userId)
        {   
            var products = await _producrService.getProductsByUserId(userId);
            return Ok(products);
        }

        [HttpGet("all-for-customer")]
        public async Task<IActionResult> getAllProductForCustomer()
        {
            var products = await _producrService.getAllProductsAvailableForCustomer();
            return Ok(products);
        }


        [HttpGet("all")]
        public async Task<IActionResult> getAllProducts()
        {
            var products = await _producrService.getAllProducts();
            return Ok(products);
        }


        [HttpGet("available-products")]
        public async Task<IActionResult> getAvailableProducts()
        {
            var products = await _producrService.getAvailableProducts();
            return Ok(products);
        }


        [Authorize(Roles ="Admin")]
        [HttpPost("add-product")]
        public async Task<IActionResult> addProduct(int userId, [FromForm]ProductDto productDto, IFormFile? productImage)
        {
            if(productDto != null && await _registerService.getUserById(userId)!= null)
            {
                var existingProduct = await _producrService.getProductByCode(productDto.productCode);
                if (existingProduct == null)
                {
                    await _producrService.addProduct(userId, productDto, productImage);
                    return Ok();
                }
                //else if( existingProduct != null)
                //{
                //    await _producrService.updateProduct(existingProduct.id, productDto, productImage);
                //}
                //return BadRequest("Already exists");
            }
            return BadRequest();
        }

        [Authorize(Roles ="Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> updateProduct(string productId,[FromForm] ProductDto productDto, IFormFile? productImage)
        {
            if(productDto!=null || productId != null)
            {
                await _producrService.updateProduct(productId, productDto, productImage);
                return Ok();
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-product")]
        public async Task<IActionResult> deleteProduct(int productId)
        {
            if (productId != null)
            {
                var result = await _producrService.deleteProduct(productId);
                if (result)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            return BadRequest();
        }
        
    }

}
