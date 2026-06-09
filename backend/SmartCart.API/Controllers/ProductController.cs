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
    private readonly ILogger<ProductController> _logger;
   //ILogger is built-in class for logging the information in dotnet core ,
   // //we just need to inject it in controller of class where we want logging
   // it log the information on debug console
   //and we need to define log level it in appSetting json file

   //We can use ILogger everyhere ,just need to inject in all service constructor/
   //Best things if If we add package serilog and configure it then ILogger will work like serilog

    // ***********
        //CORE CONCEPT (VERY IMPORTANT)
    // Serilog is NOT a replacement for ILogger
    //  It is a logging provider behind ILogger
   //Serilog logprovider use for file logging
   //*********** 

    public ProductController(IProductService iProductService, ILogger<ProductController> logger)
    {
        _iProductService = iProductService;
        _logger = logger;
    }

    [HttpGet]
    // [AllowAnonymous]
    [Route("GetAll")]
    public async Task< IActionResult> GetAllProduct(int page,int pageSize)
    {
        try
        {
            _logger.LogInformation("Start Fetching the All Products From Controller ");
            var allProduct = await _iProductService.GetAllAsync(page,pageSize);
            
            _logger.LogInformation("End Fetching the All Products From Controller ");
            return Ok(new ApiResponse<PageResponse<ProductDto>>
            {
                Success = true,
                Message ="Product Fetched",
                Data = allProduct
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurs while  Fetching the All Products From Controller ");
            return BadRequest(ex.Message);
        }
    }
    [HttpGet]
    // [AllowAnonymous]
    [Route("GetById")]
    public async Task< IActionResult> GetProductById(int productId)
    {
        try
        {
            _logger.LogInformation("Start Fetching the Product Detail From Controller ");
            var productDetail = await _iProductService.GetByIdAsync(productId);
            
            _logger.LogInformation("End Fetching the Product Detail From Controller ");
            return Ok(productDetail);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurs while  Fetching the Product Detail From Controller ");
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
            _logger.LogError("Error occurs while  adding the Products ");
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