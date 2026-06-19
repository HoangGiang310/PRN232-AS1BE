using SimpleShop.Repo.Models;
using SimpleShop.Repo.Repositories;
using SimpleShop.Service.Interfaces;

namespace SimpleShop.Service.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    private static ProductDto ToDto(Product p) => new()
    {
        ProductID = p.ProductID,
        ProductName = p.ProductName,
        Description = p.Description,
        Price = p.Price,
        StockQuantity = p.StockQuantity,
        ImageUrl = p.ImageUrl,
        CategoryID = p.CategoryID,
        CategoryName = p.Category?.CategoryName ?? "",
        IsActive = p.IsActive,
        CreatedDate = p.CreatedDate,
        ModifiedDate = p.ModifiedDate
    };

    public async Task<IEnumerable<ProductDto>> GetAllActiveAsync()
        => (await _repo.GetAllActiveAsync()).Select(ToDto);

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
        => (await _repo.GetAllAsync()).Select(ToDto);

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var p = await _repo.GetByIdAsync(id);
        return p == null ? null : ToDto(p);
    }

    public async Task<IEnumerable<ProductDto>> GetByCategoryAsync(int categoryId)
        => (await _repo.GetByCategoryAsync(categoryId)).Select(ToDto);

    public async Task<IEnumerable<ProductDto>> SearchAsync(string? name, decimal? minPrice, decimal? maxPrice, int? categoryId)
        => (await _repo.SearchAsync(name, minPrice, maxPrice, categoryId)).Select(ToDto);

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            ProductName = dto.ProductName,
            Description = dto.Description,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            ImageUrl = dto.ImageUrl,
            CategoryID = dto.CategoryID,
            IsActive = dto.IsActive,
            CreatedDate = DateTime.UtcNow
        };
        var created = await _repo.CreateAsync(product);
        var withCategory = await _repo.GetByIdAsync(created.ProductID);
        return ToDto(withCategory!);
    }

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _repo.GetByIdAsync(id);
        if (product == null) return null;

        product.ProductName = dto.ProductName;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;
        product.ImageUrl = dto.ImageUrl;
        product.CategoryID = dto.CategoryID;
        product.IsActive = dto.IsActive;

        await _repo.UpdateAsync(product);
        var updated = await _repo.GetByIdAsync(id);
        return ToDto(updated!);
    }

    public async Task<bool> SoftDeleteAsync(int id)
        => await _repo.SoftDeleteAsync(id);
}
