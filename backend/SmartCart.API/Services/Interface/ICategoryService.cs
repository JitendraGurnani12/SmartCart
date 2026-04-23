public interface ICategoryService
{
    public Task<IEnumerable<CategoryDto>> GetAllAsync();
    public Task AddAsync(CategoryDto categoryDto);
    Task<Category>GetByIdAsync(int id);
    Task Update(CategoryDto categoryDto);
    Task DeleteAsync(int id);
}