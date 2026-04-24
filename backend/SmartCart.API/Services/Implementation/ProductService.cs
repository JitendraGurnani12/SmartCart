public class ProductService : IProductService
{
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly ILogger<ProductService> _logger;
    public ProductService(IProductRepository productRepo,ICategoryRepository categoryRepo, ILogger<ProductService> logger)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _logger = logger;
    }
    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        _logger.LogInformation("Start Fetching the All Products From Product Serevice");
        IEnumerable<ProductDto> productDtoList = new List<ProductDto>();
         var products =await _productRepo.GetAllAsync();
         productDtoList= products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            CategoryId = p.CategoryId
        }).ToList();
        _logger.LogInformation("End Fetching the All Products From Product Serevice");
        return productDtoList;
    }
    public async Task AddAsync(ProductDto productDto)
    {
        _logger.LogInformation("Start Add Product in Method of Product Serevice");
        if (!IsCategoryExist(productDto.CategoryId))
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
            CreatedOn = DateTime.UtcNow,
        };
        _logger.LogInformation("End Of AddProduct method of Product Serevice");
         await _productRepo.AddAsync(product);
    }
    public Task UpdateAsync(ProductDto productDto)
    {
        _logger.LogInformation("Start Update Product in Method of Product Serevice");
        if (!IsCategoryExist(productDto.CategoryId))
        {
            _logger.LogError("Category Not Exist exception occur in update product");
            throw new Exception("Category does not exist");
        }
        var product = new Product
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Price = productDto.Price,
            Description = productDto.Description,
            CategoryId = productDto.CategoryId,
            ModifiedBy = productDto.UserId,
            ModifiedOn = DateTime.UtcNow,
        };
         _logger.LogInformation("End Update Product in Method of Product Serevice");
        return _productRepo.UpdateAsync(product);
    }
    public async Task DeleteAsync(int id)
    {
        var product = await  _productRepo.GetByIdAsync(id);
        if(product == null)
        {
            _logger.LogError("Product Not Exist exception occur in delete product");
            throw new KeyNotFoundException("Product does not exist");
        }
        product.IsDeleted = true;
        await _productRepo.UpdateAsync(product);
    }
    public bool IsCategoryExist(int categoryId)
    {
        bool result = true;
        try
        {
            var category = _categoryRepo.GetByIdAsync(categoryId);
            if(category == null)
            {
                result = false;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        return result;
    }
}