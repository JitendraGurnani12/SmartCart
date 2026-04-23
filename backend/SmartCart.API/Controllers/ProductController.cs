using System.Security.Claims;
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
    [AllowAnonymous]
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
            productDto.UserId =User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            await _iProductService.AddAsync(productDto);
            return Ok(new ApiResponse<ProductDto>()
            {
                Data = productDto,
                Success = true,
                Message = "Product Added Successfully"
            }); 
        }
        catch (Exception ex)
        {
           return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Manager")]
    [Route("Update")]
    public async Task<IActionResult> UpdateProduct(ProductDto productDto)
    {
        try
        {
            productDto.UserId =User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            await _iProductService.UpdateAsync(productDto);
            return Ok(new ApiResponse<ProductDto>()
            {
                Data = productDto,
                Success = true,
                Message = "Product Updated Successfully"
            }); 
        }
        catch (Exception ex)
        {
           return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Authorize(Roles = "Admin,Manager")]
    [Route("Delete")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            
            await _iProductService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    
}