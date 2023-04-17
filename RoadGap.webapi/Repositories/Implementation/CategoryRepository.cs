using RoadGap.webapi.Data;
using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories.Implementation;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _entityFramework;

    public CategoryRepository(IConfiguration configuration)
    {
        _entityFramework = new DataContext(configuration);
    }

    public CategoryRepository(DataContext context)
    {
        _entityFramework = context;
    }
    public void Dispose()
    {
        _entityFramework.Category.RemoveRange(_entityFramework.Category);
        SaveChanges();
    }

    public DataContext GetDataContext()
    {
        return _entityFramework;
    }
    public void SaveChanges()
    {
        if (_entityFramework.SaveChanges() < 0)
            throw new Exception("Failed to save changes to database");
    }

    public void AddEntity<T>(T entity)
    {
        if (entity == null) return;
        _entityFramework.Add(entity);
    }
    
    public void RemoveEntity<T>(T entity)
    {
        if (entity == null) return;
        _entityFramework.Remove(entity);
    }
    
    public IEnumerable<Category> GetCategories() =>
        _entityFramework.Category.ToList();

    public Category? GetCategoryById(int categoryId)
    {
        var category = _entityFramework.Category
            .FirstOrDefault(category => category.CategoryId == categoryId);
        
        return category;
    }

    public IEnumerable<Category> GetCategoriesBySearch(string searchParam)
    {
        var keywords = searchParam.ToLower().Split(' ');
        var categories = _entityFramework.Category;
        var searchedCategories = new List<Category>();
        
        foreach (var keyword in keywords)
        {
            searchedCategories.AddRange(categories.Where(category =>
                    category.Title.ToLower().Contains(keyword) ||
                    category.Description.ToLower().Contains(keyword))
                .ToList());
        }

        return searchedCategories;
    }
}