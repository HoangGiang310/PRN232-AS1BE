namespace SimpleShop.Service.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllActiveAsync();
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<IEnumerable<CategoryDto>> SearchByNameAsync(string keyword);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
    Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto);
    Task<(bool success, string message)> DeleteAsync(int id);
}
