using AutoMapper;
using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories.Implementation;

public class CategoryRepository : Repository, ICategoryRepository
{
    public CategoryRepository(IConfiguration configuration, IMapper mapper)
        : base(configuration, mapper)
    {
    }
    public void Dispose()
    {
        EntityFramework.Category.RemoveRange(EntityFramework.Category);
        SaveChanges();
    }
    public IEnumerable<Category> GetCategories() =>
        EntityFramework.Category.ToList();

    public Category? GetCategoryById(int categoryId)
    {
        var category = EntityFramework.Category
            .FirstOrDefault(category => category.CategoryId == categoryId);
        
        return category;
    }

    public IEnumerable<Category> GetCategoriesBySearch(string searchParam)
    {
        var keywords = searchParam.ToLower().Split(' ');
        var categories = EntityFramework.Category;
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