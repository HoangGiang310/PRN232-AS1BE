using SimpleShop.Repo.Models;

namespace SimpleShop.Repo.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllActiveAsync();
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<IEnumerable<Category>> SearchByNameAsync(string keyword);
    Task<Category> CreateAsync(Category category);
    Task<Category> UpdateAsync(Category category);
    Task<bool> DeleteAsync(int id);
    Task<bool> HasProductsAsync(int categoryId);
}
