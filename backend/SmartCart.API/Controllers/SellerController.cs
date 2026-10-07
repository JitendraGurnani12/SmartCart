using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SellerController : ControllerBase
{
    private readonly IOrderService _orderService;
    public SellerController(IOrderService orderService)
    {
         _orderService = orderService; 
    }
    
    [Authorize(Roles = "Seller")]
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        return Ok();
    }

    // [HttpGet("GetSellerOrders")]
    // [Authorize(Roles = "Seller")]
    // public async Task<IActionResult> GetSellerOrders()
    // {
    //     try
    //     {
    //         var sellerId =
    //             User.FindFirstValue(
    //                 ClaimTypes.NameIdentifier
    //             );

    //         // var orders =
    //         //     // await _orderService.GetSellerOrders(
    //         //     //     sellerId
    //         //     // );

    //         return Ok(
    //             new ApiResponse<List<OrderResponseDto>>
    //             {
    //                 Success = true,
    //                 Message = "Seller orders fetched successfully",
    //                 Data = orders
    //             }
    //         );
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }
    // }
}