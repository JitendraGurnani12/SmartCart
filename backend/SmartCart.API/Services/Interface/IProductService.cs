public interface IProductService
{
     Task<IEnumerable<ProductDto>> GetAllAsync();
    Task AddAsync(ProductDto productDto);
    Task UpdateAsync(ProductDto productDto);
    Task DeleteAsync(int id);
}