using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories;

public interface ICategoryRepository : IRepository
{
    public IEnumerable<Category> GetCategories();
    public Category? GetCategoryById(int categoryId);
    public IEnumerable<Category> GetCategoriesBySearch(string searchParam);
}