using Microsoft.Extensions.Caching.Memory;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepo;
    private readonly IMemoryCache _cache;
    public CategoryService(ICategoryRepository categoryRepo, IMemoryCache cache)
    {
        _categoryRepo = categoryRepo;
        _cache = cache;
    }
    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        if(!_cache.TryGetValue("categories",out List<CategoryDto> categories))
        {
            var category=  await _categoryRepo.GetAllAsync();
            
            categories = category.Select(x => new CategoryDto()
            {
                    Id = x.Id,
                    Name = x.Name
            }).ToList();
            _cache.Set("categories",categories);
        }
        return categories;
    }
    public Task AddAsync(CategoryDto categoryDto)
    {
        var category = new Category
        {
            Name = categoryDto.Name,
            ParentCategoryId = categoryDto.ParentCatgoryId,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = categoryDto.UserId,
        };
        _cache.Remove("categories");
        return _categoryRepo.AddAsync(category);
    }
    public Task<Category> GetByIdAsync(int id)
    {
        return _categoryRepo.GetByIdAsync(id);
    }

    public Task Update(CategoryDto categoryDto)
    {
        var category = new Category
        {
            Id = categoryDto.Id,
            Name = categoryDto.Name,
            ParentCategoryId = categoryDto.ParentCatgoryId,
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = categoryDto.UserId,
        };
        _cache.Remove("categories");
        return _categoryRepo.Update(category);
    }
    public async Task DeleteAsync(int id)
    {
        var category = await  _categoryRepo.GetByIdAsync(id);
        if(category == null)
        {
            throw new KeyNotFoundException("Category does not exist");
        }
        category.IsDeleted = true;
        _cache.Remove("categories");
        await _categoryRepo.Update(category);
    }
}