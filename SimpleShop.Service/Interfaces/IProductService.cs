namespace SimpleShop.Service.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllActiveAsync();
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task<IEnumerable<ProductDto>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<ProductDto>> SearchAsync(string? name, decimal? minPrice, decimal? maxPrice, int? categoryId);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto);
    Task<bool> SoftDeleteAsync(int id);
}
