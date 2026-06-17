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

    [HttpGet("GetByUserId")]
    public async Task<IActionResult> GetOrderByUserId()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _orderService.GetOrderByUserId(userId);

            return Ok(new ApiResponse<List<OrderResponseDto>>
            {
                Success = true,
                Message = "Orders Fetched Successfully",
                Data = orders
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("GetOrderDetailById")]
    public async Task<IActionResult> GetOrderDetailById(int id)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderService.GetOrderDetailById(id, userId);

            return Ok(new ApiResponse<OrderResponseDto>
            {
                Success = true,
                Message = "Order detail fetched successfully",
                Data = order
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
}