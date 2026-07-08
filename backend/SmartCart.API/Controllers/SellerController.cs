using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SellerController : ControllerBase
{
    
    public SellerController()
    {
        
    }
    
    [Authorize(Roles = "Seller")]
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        return Ok();
    }
}