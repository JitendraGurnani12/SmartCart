using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class OrderController: ControllerBase
{
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;   
    }

    [HttpPost]
    [Route("PlaceOrder")]
    public async Task<IActionResult> AddOrder(OrderDto orderDto)
    {
        try
        {
            orderDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _orderService.AddOrder(orderDto);
            return Ok();

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}