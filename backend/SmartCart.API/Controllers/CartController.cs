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
    public  async Task<IActionResult> Get()
    {
        
                
            return Ok();
            
       
    }

    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> AddItemIntoCart(CartItemDto cartItemDto)
    {
        
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            cartItemDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           await _cartService.AddItemIntoCart(cartItemDto);
            return Ok();
        
        
    }

    [HttpPut]
    [Route("Update")]
    public async Task<IActionResult> UpdateItemIntoCart()
    {
        
            return Ok();
        
       
    }

    [HttpDelete]
    [Route("Remove")]
    public async Task<IActionResult> RemoveItemFromCart()
    {
        
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