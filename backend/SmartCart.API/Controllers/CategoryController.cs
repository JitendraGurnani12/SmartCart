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
            return Ok(allCategory);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles ="Admin")]
    [Route("Add")]
    public async Task<IActionResult> AddNewCategory([FromBody]CategoryDto categoryDto) //FromBody is by default if we use [ApiController] attribute on above of Controller Name// 
    {
        try
        {
            await _iCategoryService.AddAsync(categoryDto);
            return Ok(categoryDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    
}