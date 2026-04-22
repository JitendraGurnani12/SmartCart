using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _iProductService;

    public ProductController(IProductService iProductService)
    {
        _iProductService = iProductService;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task< IActionResult> GetAllProduct()
    {
        try
        {
            var allProduct = await _iProductService.GetAllAsync();
            // return Ok(allProduct);
            return Ok(new ApiResponse<IEnumerable<ProductDto>>()
            {
                Success = true,
                Message ="Product Fetched",
                Data = allProduct
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    [Route("Add")]
    public async Task<IActionResult> AddNewProduct(ProductDto productDto)
    {
        try
        {
            await _iProductService.AddAsync(productDto);
            return Ok(productDto); 
        }
        catch (Exception ex)
        {
           return BadRequest(ex.Message);
        }
    }

    
}