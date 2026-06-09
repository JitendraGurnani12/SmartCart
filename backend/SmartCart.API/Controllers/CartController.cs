using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {

                _cartService = cartService;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
                var carts = await _cartService.GetCarts();
                return Ok(carts);
        }

        [HttpGet]
        [Route("GetCartByUserId")]
        public async Task<IActionResult> GetCartByUserId()
        {
                var userId =
                    User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? string.Empty;

                var cart =
                    await _cartService.GetCartByUserId(userId);

                if (cart == null)
                {
                        return NoContent();
                }

                return Ok(new ApiResponse<CartResponseDto>
                {
                        Success = true,
                        Message = "Cart Fetched Successfully",
                        Data = cart
                });
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddItemIntoCart(CartItemDto cartItemDto)
        {

                cartItemDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _cartService.AddItemIntoCart(cartItemDto);
                return Ok();
        }

        // [HttpPut]
        // [Route("Update")]
        // public async Task<IActionResult> UpdateItemIntoCart()
        // {
        //         return Ok();
        // }

        [HttpDelete]
        [Route("RemoveItem")]
        public async Task<IActionResult> RemoveItemFromCart(CartItemDto cartItemDto)
        {

                cartItemDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _cartService.RemoveItemFromCart(cartItemDto);
                return Ok();
        }
        [HttpPut]
        [Route("UpdateQuantity")]
        public async Task<IActionResult> UpdateQuantityInCart(UpdateCartItemDto dto)
        {
                // cartItemDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _cartService.UpdateItemQuantityIntoCart(dto);
                return Ok();
                
        }



        /*
        📡 APIs YOU MUST BUILD
    1. Add to Cart
    POST /api/cart/add
    2. Get Cart
    GET /api/cart
    3. Update Quantity
    PUT /api/cart/update
    4. Remove Item
    DELETE /api/cart/remove/{id} 
        */
}