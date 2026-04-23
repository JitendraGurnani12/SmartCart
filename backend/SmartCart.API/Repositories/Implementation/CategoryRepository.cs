using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;

    public CategoryRepository(AppDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        if (! _cache.TryGetValue("categories",out IEnumerable<Category> Categories))
        {
            Categories = await _context.Categories.Where(p=>!p.IsDeleted).ToListAsync();
            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

            _cache.Set("categories",Categories,cacheOptions);
            ///// IMemoryCache.Set<IEnumerable<Category>>(object key, IEnumerable<Category> value, 
            /// MemoryCacheEntryOptions? options)
        }
        return Categories;
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        return await _context.Categories.FirstOrDefaultAsync(c=>c.Id == id && !c.IsDeleted);
    }

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        _cache.Remove("categories");
        await _context.SaveChangesAsync();
    }
    public async Task Update(Category category)
    {
        _context.Categories.Update(category);
        _cache.Remove("categories");
        await _context.SaveChangesAsync();
    }
    // public async Task DeleteCategory()
    // {
    //     await _context
    // }
}