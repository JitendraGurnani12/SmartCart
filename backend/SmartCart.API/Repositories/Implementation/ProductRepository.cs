using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;
    public ProductRepository(AppDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        if (! _cache.TryGetValue("products",out IEnumerable<Product>products))
        {
            products = await _context.Products.Where(p=>!p.IsDeleted).ToListAsync();
            var cacheOptions = new MemoryCacheEntryOptions()
                                .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

            _cache.Set("products",products,cacheOptions);
        }
        return  products ;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _context.Products.FirstOrDefaultAsync(p=>p.Id == id && !p.IsDeleted);
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        _cache.Remove("products");
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
         _context.Products.Update(product);
         _cache.Remove("products");
        await _context.SaveChangesAsync();
    }
}