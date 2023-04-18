using AutoMapper;
using RoadGap.webapi.Data;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Helpers;
using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories.Implementation;

public class CategoryRepository : Repository, ICategoryRepository
{
    public CategoryRepository(IConfiguration configuration, IMapper mapper)
        : base(configuration, mapper)
    {
    }
    
    public CategoryRepository(DataContext context, IMapper mapper) 
        : base(context, mapper)
    {
    }
    
    public void Dispose()
    {
        EntityFramework.Category.RemoveRange(EntityFramework.Category);
        SaveChanges();
    }

    public RepositoryResponse<IEnumerable<Category>> GetCategories(string searchParam = "")
    {
        searchParam = searchParam.Trim();
        if (searchParam == "")
        {
            return RepositoryResponse<IEnumerable<Category>>
                .CreateSuccess(EntityFramework.Category.ToList(),
                    "Categories found successfully.");
        }

        var keywords = searchParam.ToLower().Split(' ');

        bool SearchPredicate(Category category) => keywords.Any(keyword =>
            category.Title.ToLower().Contains(keyword) || category.Description.ToLower().Contains(keyword));

        return SearchEntities((Func<Category, bool>)SearchPredicate, "Categories searched successfully.",
            "An error occurred while getting categories.");
    }
    public RepositoryResponse<Category> GetCategoryById(int categoryId)
    {
        return GetEntityById<Category>(categoryId);
    }
    public RepositoryResponse<Category> EditCategory(int categoryId, CategoryToUpsertDto categoryDto)
    {
        return EditEntity(categoryId, categoryDto, GetCategoryById);
    }
    public RepositoryResponse<Category> CreateCategory(CategoryToUpsertDto categoryToAdd)
    {
        return CreateEntity<Category, CategoryToUpsertDto>(categoryToAdd);
    }
    public RepositoryResponse<int> DeleteCategory(int categoryId)
    {
        try
        {
            var response = GetCategoryById(categoryId);

            if (!response.Success)
            {
                return RepositoryResponse<int>.CreateBadRequest($"Category with ID {categoryId} not found");
            }

            var category = response.Data;

            RemoveEntity(category);
            SaveChanges();

            return RepositoryResponse<int>.CreateSuccess(categoryId, "Category deleted successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<int>.CreateInternalServerError($"An error occurred while editing category with ID {categoryId}: {ex.Message}");
        }
    }
}