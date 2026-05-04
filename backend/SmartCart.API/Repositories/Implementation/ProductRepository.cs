using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
            products = await _context.Products.Where(p=>!p.IsDeleted).OrderBy(p=>p.Id).ToListAsync();
            var cacheOptions = new MemoryCacheEntryOptions()
                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

                              

            _cache.Set("products",products,cacheOptions);
        }
        return  products ;
    }
    public IQueryable<Product> GetProductQuery()
    {
        return _context.Products.Where(p=>!p.IsDeleted).OrderBy(p=>p.Id);   
    }
    /* *****Just small information about caching , this is not full info about caching****

    Caching --Caching is performance boost technique where frequent asked data is stored in tempraroy storage.
    --------------Instead API hit the data base and get the data and slow down the server , 
    --------------API will get data from cache

    ****************************** types **************************** ---------

    1.InMemory caching (we are using now)--- data will stored on RAM where API is runnig on server

    2.Distributed Caching-- we use external service ,which will connected to our application like REdis or Memcached

    3.Response caching -- this caching will cached the entire HTTP response instead of object or data row , 
    we can use it by using Cache-Control header and  [ResponseCache] attribute in controller
    
    4.Client side Browser caching--the data is stored directly in user browser

    ********************How to Implement InMemoryCache******************************
    
    -----register the service in Program.cs 
    -----using builder.Services.AddMemoryCache(); and then inject 
    IMemoryCache into your controller or service

    Set() and Get() Method

        Set: Adds an item to the cache with a specific key.

        Get: Retrieves the item. If the key doesn't exist, it returns null.
    Cache Expiration ----new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
    By using Absolute expire cache will expire after minutes when you create it

    */

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _context.Products.FirstOrDefaultAsync(p=>p.Id == id && !p.IsDeleted);
    }
    public  IQueryable<Product> GetProductQueryById(int id)
    {
        return   _context.Products.Where(p=>!p.IsDeleted && p.Id == id);
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