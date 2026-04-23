public class ProductService : IProductService
{
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;
    public ProductService(IProductRepository productRepo,ICategoryRepository categoryRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
    }
    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
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
        return productDtoList;
    }
    public Task AddAsync(ProductDto productDto)
    {
        if (!IsCategoryExist(productDto.CategoryId))
        {
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
        return _productRepo.AddAsync(product);
    }
    public Task UpdateAsync(ProductDto productDto)
    {
        if (!IsCategoryExist(productDto.CategoryId))
        {
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
        return _productRepo.UpdateAsync(product);
    }
    public async Task DeleteAsync(int id)
    {
        var product = await  _productRepo.GetByIdAsync(id);
        if(product == null)
        {
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