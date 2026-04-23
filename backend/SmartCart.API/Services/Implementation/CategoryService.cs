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
            ParentCategoryId = categoryDto.ParentCatgoryId,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = categoryDto.UserId,
        };
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
        await _categoryRepo.Update(category);
    }
}