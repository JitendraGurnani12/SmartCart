public interface ICategoryService
{
    public Task<IEnumerable<CategoryDto>> GetAllAsync();
    public Task AddAsync(CategoryDto categoryDto);
    Task<Category>GetByIdAsync(int id);
}