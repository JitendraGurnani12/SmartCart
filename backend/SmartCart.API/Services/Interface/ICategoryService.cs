public interface ICategoryService
{
    public Task<IEnumerable<Category>> GetAllAsync();
    public Task AddAsync(CategoryDto categoryDto);
    Task<Category>GetByIdAsync(int id);
}