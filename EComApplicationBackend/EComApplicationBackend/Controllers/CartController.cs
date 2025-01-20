using App.Core.Dto;
using App.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EComApplicationBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartDetailService _cartDetailService;
        private readonly ICartService _cartService;

        public CartController(ICartService cartService, ICartDetailService cartDetailService)
        {
            _cartDetailService = cartDetailService;
            _cartService = cartService;
        }


        [Authorize(Roles = "Customer")]
        [HttpGet("get-cart-items")]
        public async Task<IActionResult> getAllCart(int cartId)
        {
            if(cartId != null || cartId != 0)
            {
                var cartItems = await _cartService.getCartItems(cartId);
                return Ok(cartItems);
            }
            return BadRequest();
        }
        [Authorize(Roles = "Customer")]
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> addProductToCart(int userId, CartProductDto cart)
        {
            if(cart != null &&  userId != 0)
            {
                var result = await _cartDetailService.addItemToCart(userId, cart);
                if (result)
                {
                    return Ok(cart);
                }
                else
                {
                    return BadRequest();
                }
                
            }
            return BadRequest();
        }
        [Authorize(Roles = "Customer")]
        [HttpPut("reduce-quantity")]
        public async Task<IActionResult> reduceQuantity(int cartId, int productId)
        {
            if(cartId != 0 && productId!=0)
            {
                var result = await _cartDetailService.reduceQuantity(cartId, productId);
                if (result)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            return BadRequest();
        }
        [Authorize(Roles = "Customer")]
        [HttpDelete("remove-item")]
        public async Task<IActionResult> removeItemFromCart(int cartId, int productId)
        {
            if(cartId != 0 && productId !=0)
            {
                var result  = await _cartDetailService.removeItemFromCart(cartId, productId); ;
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
