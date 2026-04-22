public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepo;
    public CategoryService(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }
    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
       var category=  await _categoryRepo.GetAllAsync();
       return category.Select(x => new CategoryDto()
       {
            Id = x.Id,
            Name = x.Name
       });
    }
    public Task AddAsync(CategoryDto categoryDto)
    {
        var category = new Category
        {
            Name = categoryDto.Name,
            ParentCategoryId = categoryDto.ParentCatgoryId
        };
        return _categoryRepo.AddAsync(category);
    }
    public Task<Category> GetByIdAsync(int id)
    {
        return _categoryRepo.GetByIdAsync(id);
    }
}