using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _iCategoryService;

    public CategoryController(ICategoryService iCategoryService)
    {
        _iCategoryService = iCategoryService;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAllCategory()
    {
        try
        {

            var allCategory = await _iCategoryService.GetAllAsync();
            // return Ok(allCategory);
            return Ok(new ApiResponse<IEnumerable<CategoryDto>>()
            {
                Success = true,
                Message = "Category Fetched Successfully",
                Data = allCategory
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles ="Admin,Manager")]
    [Route("Add")]
    public async Task<IActionResult> AddNewCategory([FromBody]CategoryDto categoryDto) 
    {
        try
        {
            //////FromBody is by default if we use [ApiController] attribute on above of Controller Name// 
            categoryDto.UserId =User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            await _iCategoryService.AddAsync(categoryDto);
            return Ok(new ApiResponse<CategoryDto>()
            {
                Success = true,
                Message="Category Added Successfully",
                Data = categoryDto
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut]
    [Route("Update")]
    public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto categoryDto)
    {
        try
        {
            categoryDto.UserId =User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            await _iCategoryService.Update(categoryDto);
            return Ok(new ApiResponse<CategoryDto>()
            {
                Success = true,
                Data = categoryDto,
                Message = "Category Updated Successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete]
    [Route("Delete")]
    [Authorize(Roles ="Admin,Manager")]
    public async  Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            await _iCategoryService.DeleteAsync(id);
            return Ok();
            
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }

    
}