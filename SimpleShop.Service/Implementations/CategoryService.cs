using SimpleShop.Repo.Models;
using SimpleShop.Repo.Repositories;
using SimpleShop.Service.Interfaces;

namespace SimpleShop.Service.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo)
    {
        _repo = repo;
    }

    private static CategoryDto ToDto(Category c) => new()
    {
        CategoryID = c.CategoryID,
        CategoryName = c.CategoryName,
        CategoryDescription = c.CategoryDescription,
        IsActive = c.IsActive,
        ProductCount = c.Products?.Count ?? 0
    };

    public async Task<IEnumerable<CategoryDto>> GetAllActiveAsync()
        => (await _repo.GetAllActiveAsync()).Select(ToDto);

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        => (await _repo.GetAllAsync()).Select(ToDto);

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        return c == null ? null : ToDto(c);
    }

    public async Task<IEnumerable<CategoryDto>> SearchByNameAsync(string keyword)
        => (await _repo.SearchByNameAsync(keyword)).Select(ToDto);

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            CategoryName = dto.CategoryName,
            CategoryDescription = dto.CategoryDescription,
            IsActive = dto.IsActive
        };
        var created = await _repo.CreateAsync(category);
        return ToDto(created);
    }

    public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category == null) return null;

        category.CategoryName = dto.CategoryName;
        category.CategoryDescription = dto.CategoryDescription;
        category.IsActive = dto.IsActive;

        var updated = await _repo.UpdateAsync(category);
        return ToDto(updated);
    }

    public async Task<(bool success, string message)> DeleteAsync(int id)
    {
        var exists = await _repo.GetByIdAsync(id);
        if (exists == null) return (false, "Category not found.");

        bool hasProducts = await _repo.HasProductsAsync(id);
        if (hasProducts)
            return (false, "Cannot delete category because it has linked products.");

        await _repo.DeleteAsync(id);
        return (true, "Category deleted successfully.");
    }
}
