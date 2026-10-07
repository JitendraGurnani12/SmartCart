public interface IProductService
{
    //  Task<IEnumerable<ProductDto>> GetAllAsync(int page,int pageSize);
    Task AddAsync(ProductDto productDto);
    Task UpdateAsync(ProductDto productDto);
    Task DeleteAsync(int id,string userId,bool isAdmin);
    Task<PageResponse<ProductDto>> GetAllAsync(int page, int pageSize, string? search, int? categoryId);
    Task<PageResponse<ProductDto>> GetSellerProductsAsync(string sellerId,int page, int pageSize, string? search, int? categoryId);
    Task<ProductDto> GetByIdAsync(int id);
    Task<ProductDto> GetMyProductByIdAsync(string sellerId,int id);
}