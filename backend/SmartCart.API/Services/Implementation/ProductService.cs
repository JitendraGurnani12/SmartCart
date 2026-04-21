public class ProductService : IProductService
{
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;
    public ProductService(IProductRepository productRepo,ICategoryRepository categoryRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
    }
    public Task<IEnumerable<Product>> GetAllAsync()
    {
       return  _productRepo.GetAllAsync();
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
        };
        return _productRepo.AddAsync(product);
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