public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category> GetByIdAsync(int id);
    Task AddAsync(Category category);
    Task Update(Category category);
    IQueryable<Category> GetCategoryQueryId(int id);
    IQueryable<Category> GetCategoriesQuery();
}