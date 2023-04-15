using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories;

public interface ICategoryRepository
{
    public void SaveChanges();
    public void AddEntity<T>(T entity);
    public void RemoveEntity<T>(T entity);
    public bool StatusExists(int statusId);
    public IEnumerable<Category> GetCategories();
    public Category? GetCategoryById(int categoryId);
    public IEnumerable<Category> GetCategoriesBySearch(string searchParam);
}