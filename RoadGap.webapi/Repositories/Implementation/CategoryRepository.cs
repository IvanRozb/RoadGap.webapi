using AutoMapper;
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
    public void Dispose()
    {
        EntityFramework.Category.RemoveRange(EntityFramework.Category);
        SaveChanges();
    }
    public RepositoryResponse<IEnumerable<Category>> GetCategories(string searchParam = "") {
        try
        {
            searchParam = searchParam.Trim();
            if (searchParam == "")
            {
                return RepositoryResponse<IEnumerable<Category>>
                    .CreateSuccess(EntityFramework.Category.ToList(),
                        "Categories found successfully.");
            }

            var keywords = searchParam.ToLower().Split(' ');
            var searchedCategories = EntityFramework.Category.AsEnumerable()
                .Where(category => keywords.Any(keyword =>
                    category.Title.ToLower().Contains(keyword) ||
                    category.Description.ToLower().Contains(keyword)))
                .ToList();

            return RepositoryResponse<IEnumerable<Category>>
                .CreateSuccess(searchedCategories,
                    "Categories searched successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<IEnumerable<Category>>
                .CreateInternalServerError($"An error occurred while getting categories: {ex.Message}");
        }
    }
    public RepositoryResponse<Category> GetCategoryById(int categoryId)
    {
        try
        {
            var category = EntityFramework.Category
                .FirstOrDefault(category => category.CategoryId == categoryId);

            if (category == null)
            {
                return RepositoryResponse<Category>.CreateNotFound($"Category with ID {categoryId} not found");
            }

            return RepositoryResponse<Category>
                .CreateSuccess(category, "Category found successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<Category>.CreateInternalServerError($"An error occurred while getting category with ID {categoryId}: {ex.Message}");
        }
    }
    public RepositoryResponse<Category> EditCategory(int categoryId, CategoryToUpsertDto categoryDto)
    {
        try
        {
            var categoryResponse = GetCategoryById(categoryId);

            if (!categoryResponse.Success)
            {
                return categoryResponse;
            }

            var category = categoryResponse.Data;
            
            Mapper.Map(categoryDto, category);

            EntityFramework.SaveChanges();

            return RepositoryResponse<Category>.CreateSuccess(category, "Category updated successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<Category>.CreateInternalServerError($"An error occurred while editing category with ID {categoryId}: {ex.Message}");
        }
    }
    public RepositoryResponse<Category> CreateCategory(CategoryToUpsertDto categoryToAdd)
    {
        try
        {
            var category = Mapper.Map<Category>(categoryToAdd);
            
            AddEntity(category);
            SaveChanges();

            return RepositoryResponse<Category>.CreateSuccess(category, "Category created successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<Category>.CreateInternalServerError($"An error occurred while creating category: {ex.Message}");
        }
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