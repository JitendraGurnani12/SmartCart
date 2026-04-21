public interface IProductService
{
    Task<IEnumerable<Product>>GetAllAsync();
    Task AddAsync(ProductDto productDto);
}