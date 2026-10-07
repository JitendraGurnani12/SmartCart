using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly ILogger<ProductService> _logger;
    private readonly IMemoryCache _cache;
    private readonly IWebHostEnvironment _environment;
    /*
    For My Understanding I have copied from Googlegemini ,jsut for reading............
    
    Main Understanding of Async and await ,Please read below details slowly for understand it
    -----------------
    In C#, async and await are keywords used to write asynchronous code, allowing your application to stay 
    responsive while performing long-running tasks like downloading data, reading files, or accessing databases. 
    -----------------
    The Core Concept
    Think of it like a chef in a kitchen. Instead of standing idle waiting for water to boil (synchronous), 
    the chef starts the stove and moves on to chop vegetables (asynchronous). 
    ----------------

    async: This keyword is added to a method's signature to indicate it contains asynchronous work. 
    It allows you to use the await keyword inside that method.
    await: This is used before a task. It tells the program: "Pause this specific method here 
    until the task is done, but feel free to go do other work elsewhere in the meantime

    *
    *

    *
    *
   ****** Is method ke flow ko simple steps mein samjhte hain ki await yahan kya kar raha hai:*********
1. await IsCategoryExist(...) par kya hota hai?
    Jab code is line par aata hai, toh check hota hai ki Category database mein hai ya nahi.
    Method ke andar (Pause): Ye method yahan ruk jayega. Jab tak database se confirmation nahi aata ki 
    Category exist 
    karti hai ya nahi, tab tak niche wali var product = ... wali lines execute nahi hongi. 
    Kyunki agar category hi nahi hai, toh product banane ka fayda nahi.
    App ke liye (Move Forward): Is "waiting time" ke dauran aapka server ya application free hai. Wo 
    doosre users ki requests handle kar sakta hai. Thread free ho jata hai.
2. Product Object Creation
    Jab IsCategoryExist ka result aa jata hai (maan lo true), tab code aage badhta hai aur 
    var product = new Product { ... } wala kaam karta hai. Ye memory mein ho raha hai, isliye iske liye 
    await ki zaroorat nahi hai.
3. await _productRepo.AddAsync(product) par kya hota hai?
    Ab final step hai product ko database mein save karna.
    Pause: Method phir se yahan wait karega jab tak database save confirm nahi kar deta. 
            Method ki aakhri 
            line tabhi complete hogi jab data base mein chala jayega.
    Efficiency: Agar aap yahan await nahi lagate, toh method turant khatam ho jata, aur shayad database mein 
                entry hone se pehle hi program aage badh jata, jo error de sakta hai.

Summary (Desi Style):

    Pehla Await: "Bhai pehle Category check karke aa, tabhi main aage ka form bharunga. Tab tak main (thread) doosre kaam kar leta hoon."
    Beech ka kaam: Form bhar diya (Object create kar liya).
    Doosra Await: "Ab ye data database mein daal kar aa, jab tu 'Done' bolega tabhi ye 
    method complete mana jayega."
Ek important baat: Agar aapne _productRepo.AddAsync(product) ke pehle await nahi lagaya, toh log file mein "End Of AddProduct..." toh print ho jayega, 
lekin ho sakta hai product database mein save hone se pehle hi method return ho jaye. Isliye await lagana zaroori hai

*

*

*

*

*




**********************************************************************
*******************************Bilkul sahi! Simple words mein await = Pause.
Lekin ye ek "Smart Pause" hai. Iska matlab samajhne ke liye ye 2 points dhyan mein rakho:
Method ke liye Pause: Us method ke andar, code aage nahi badhega. Jab tak wo task (jaise database check) 
khatam nahi hota, niche wali lines execute nahi hongi.
CPU/Thread ke liye NO Pause: Jo thread (worker) us code ko chala raha tha, wo wahan khada hokar wait 
nahi karega. Wo free ho jayega aur doosre kaam (jaise UI responsive rakhna ya doosri requests handle karna)
 karne lagega.
Example:
Jaise aapne restaurant mein waiter ko order diya (await).
Aapka order (method) pause ho gaya hai jab tak khana nahi aata.
Lekin waiter (thread) pause nahi hua, wo doosre customers ko paani de raha hai.
Jaise hi khana ready hoga, waiter wapas aayega aur aapka order (method) wahi se resume ho jayega jahan pause hua tha.
        */
    public ProductService(IProductRepository productRepo,ICategoryRepository categoryRepo, ILogger<ProductService> logger,
    IWebHostEnvironment environment,IMemoryCache cache)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _environment = environment;
        _logger = logger;
        _cache = cache;
    }
    // public async Task<IEnumerable<ProductDto>> GetAllAsync()
    // {
    //     _logger.LogInformation("Start Fetching the All Products From Product Serevice");
    //     IEnumerable<ProductDto> productDtoList = new List<ProductDto>();
    //      var products =await _productRepo.GetAllAsync();
    //      productDtoList= products.Select(p => new ProductDto
    //     {
    //         Id = p.Id,
    //         Name = p.Name,
    //         Price = p.Price,
    //         Description = p.Description,
    //         CategoryId = p.CategoryId
    //     }).ToList();
    //     _logger.LogInformation("End Fetching the All Products From Product Serevice");
    //     return productDtoList;
    // }
    public async Task<PageResponse<ProductDto>> GetAllAsync(int page, int pageSize, string? search,int? categoryId)
    {
        _logger.LogInformation("Fetching products");

        // page = page <= 0 ? 1 : page;
        // pageSize = pageSize > 50 ? 50 : pageSize;
        pageSize = pageSize <= 0 ? 8 : pageSize;
        pageSize = pageSize > 50 ? 50 : pageSize;

        var query = _productRepo.GetProductQuery();
        // Apply search
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(p =>
                p.Name.ToLower().Contains(search.ToLower()) ||
                p.Description.ToLower().Contains(search.ToLower()));
        }
        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(p =>p.CategoryId == categoryId.Value); 
        }

        var totalCount = await query.CountAsync<Product>();

        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var productDtoList = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            ImageUrl = p.ImageUrl,
            SellerId = p.SellerId,
            CategoryId = p.CategoryId
        }).ToList();
        foreach(var product in productDtoList)
        {
            product.ImageUrl = $"http://localhost:5096{product.ImageUrl}";
        }

        _logger.LogInformation("Products fetched successfully");

        return new PageResponse<ProductDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalRecords = totalCount,
            Data = productDtoList
        };
    }
    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var cacheKey= $"product_{id}";
        if (! _cache.TryGetValue(cacheKey,out ProductDto productDto))
        {
            var product = await _productRepo.GetProductQueryById(id).FirstOrDefaultAsync();
            if(product == null)
            {
                throw new Exception("Product not found");
            }
            productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                SellerId = product.SellerId,
                CategoryId = product.CategoryId,
                Description = product.Description,
                ImageUrl = $"http://localhost:5096{product.ImageUrl}"
            };
            _cache.Set(cacheKey,productDto,TimeSpan.FromMinutes(15));

        }
        return productDto;
    }
    public async Task AddAsync(ProductDto productDto)
    {
        _logger.LogInformation("Start Add Product in Method of Product Serevice");

        
        if (!await IsCategoryExist(productDto.CategoryId))
        {
            _logger.LogError("Category Not Exist exception occur in add product");
            throw new Exception("Category does not exist");
        }
        var product = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
            Description = productDto.Description,
            CategoryId = productDto.CategoryId,
            CreatedBy = productDto.UserId,
            ImageUrl = productDto.ImageUrl,
            SellerId = productDto.SellerId,
            CreatedOn = DateTime.UtcNow,
        };
        _logger.LogInformation("End Of AddProduct method of Product Serevice");
         await _productRepo.AddAsync(product);
        _cache.Remove($"product_{product.Id}");
    }
    public async Task UpdateAsync(ProductDto productDto)
    {
        _logger.LogInformation("Start Update Product in Method of Product Serevice");
        var product = await _productRepo.GetByIdAsync(productDto.Id);
        
        if (product == null)
        {
            throw new KeyNotFoundException("Product does not exist");
        }
         if (product.SellerId != productDto.SellerId)
        {
            throw new UnauthorizedAccessException(
                "You are not authorized to update this product.");
        }
        if (!await IsCategoryExist(productDto.CategoryId))
        {
            _logger.LogError("Category Not Exist exception occur in update product");
            throw new Exception("Category does not exist");
        }
        // product = new Product
        // {
        //     Id = productDto.Id,
        //     Name = productDto.Name,
        //     Price = productDto.Price,
        //     Description = productDto.Description,
        //     CategoryId = productDto.CategoryId,
        //     ModifiedBy = productDto.UserId,
        //     ModifiedOn = DateTime.UtcNow,
        // };
        product.Name = productDto.Name;
        product.Price = productDto.Price;
        product.Description = productDto.Description;
        product.CategoryId = productDto.CategoryId;
        product.ImageUrl = productDto.ImageUrl;

        product.ModifiedBy = productDto.UserId;
        product.ModifiedOn = DateTime.UtcNow;
        _logger.LogInformation("End Update Product in Method of Product Serevice");
        await _productRepo.UpdateAsync(product);
        _cache.Remove($"product_{product.Id}");
    }
    public async Task DeleteAsync(int id,string userId,bool isAdmin)
    {
        var product = await  _productRepo.GetByIdAsync(id);

        if(product == null)
        {
            _logger.LogError("Product Not Exist exception occur in delete product");
            throw new KeyNotFoundException("Product does not exist");
        }
        if (!isAdmin && product.SellerId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this product.");
        }
        var imageUrl = product.ImageUrl;
        product.IsDeleted = true;
        await _productRepo.UpdateAsync(product);
        
        _cache.Remove($"product_{product.Id}");
         DeleteProductImage(imageUrl);
    }
    public async Task<bool> IsCategoryExist(int categoryId)
    {
        var category = await _categoryRepo.GetByIdAsync(categoryId);
        return category != null;
    }

    public async Task<PageResponse<ProductDto>> GetSellerProductsAsync(string sellerId,int page,int pageSize,string? search, int? categoryId)
    {
        var query = _productRepo
            .GetProductQuery()
            .Where(p => p.SellerId == sellerId);

        // page = page <= 0 ? 1 : page;
        // pageSize = pageSize > 50 ? 50 : pageSize;
        pageSize = pageSize <= 0 ? 8 : pageSize;
        pageSize = pageSize > 50 ? 50 : pageSize;
        
        // Apply search
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(p =>
                p.Name.ToLower().Contains(search.ToLower()) ||
                p.Description.ToLower().Contains(search.ToLower()));
        }
        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(p =>p.CategoryId == categoryId.Value); 
        }

        var totalCount = await query.CountAsync<Product>();

        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var productDtoList = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            ImageUrl = p.ImageUrl,
            SellerId = p.SellerId,
            CategoryId = p.CategoryId
        }).ToList();
        foreach(var product in productDtoList)
        {
            product.ImageUrl = $"http://localhost:5096{product.ImageUrl}";
        }

        _logger.LogInformation("Products fetched successfully");

        return new PageResponse<ProductDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalRecords = totalCount,
            Data = productDtoList
        }; 
    }

    public async Task<ProductDto> GetMyProductByIdAsync(string sellerId,int id)
    {
        
            var product = await _productRepo.GetProductQueryById(id).Where(p => p.SellerId == sellerId).FirstOrDefaultAsync();

            
            if(product == null)
            {
                throw new Exception("Product not found or you are not authorize to update this product");
            }
            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                SellerId = product.SellerId,
                Description = product.Description,
                ImageUrl = $"http://localhost:5096{product.ImageUrl}"
            };
            

       
        return productDto;
    }
    private void DeleteProductImage(string? imageUrl) 
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return;
        }
        var fileName =  Path.GetFileName(imageUrl);
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return;
        }
        var uploadsFolder =  Path.Combine( _environment.WebRootPath,"uploads","products");

        var filePath = Path.Combine(uploadsFolder,fileName );

        if (File.Exists(filePath))
        {
            File.Delete(filePath);

            _logger.LogInformation("Product image deleted: {FilePath}",filePath);
        }
    }
}
