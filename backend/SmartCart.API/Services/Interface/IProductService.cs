public interface IProductService
{
    //  Task<IEnumerable<ProductDto>> GetAllAsync(int page,int pageSize);
    Task AddAsync(ProductDto productDto);
    Task UpdateAsync(ProductDto productDto);
    Task DeleteAsync(int id);
    Task<PageResponse<ProductDto>> GetAllAsync(int page, int pageSize);
}