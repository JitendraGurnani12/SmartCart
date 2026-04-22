public interface IProductService
{
     Task<IEnumerable<ProductDto>> GetAllAsync();
    Task AddAsync(ProductDto productDto);
}