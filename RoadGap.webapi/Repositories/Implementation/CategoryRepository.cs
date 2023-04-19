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
            category.Title.ToLower().Contains(keyword) ||
            category.Description.ToLower().Contains(keyword));

        return SearchEntities((Func<Category, bool>)SearchPredicate,
            "Categories searched successfully.",
            "An error occurred while getting categories.");
    }

    public RepositoryResponse<Category> GetCategoryById(int categoryId)
    {
        return GetEntityById<Category>(categoryId);
    }

    public RepositoryResponse<Category> EditCategory(int categoryId, CategoryToUpsertDto categoryToEdit)
    {
        if (categoryToEdit.Title.Length > 50)
        {
            return RepositoryResponse<Category>
                .CreateConflict("Title must be 50 characters or less.");
        }

        if (categoryToEdit.Description.Length > 300)
        {
            return RepositoryResponse<Category>
                .CreateConflict("Description must be 300 characters or less.");
        }

        if (!Validator.ValidateUrl(categoryToEdit.Photo))
        {
            return RepositoryResponse<Category>
                .CreateConflict("Invalid photo URL. " +
                                "The URL must be 255 characters or less and in a valid format. " +
                                "Please use a valid URL or a shortening service like bit.ly.");
        }

        return EditEntity(categoryId, categoryToEdit, GetCategoryById);
    }

    public RepositoryResponse<Category> CreateCategory(CategoryToUpsertDto categoryToAdd)
    {
        if (categoryToAdd.Title.Length > 50)
        {
            return RepositoryResponse<Category>
                .CreateConflict("Title must be 50 characters or less.");
        }

        if (categoryToAdd.Description.Length > 300)
        {
            return RepositoryResponse<Category>
                .CreateConflict("Description must be 300 characters or less.");
        }

        if (!Validator.ValidateUrl(categoryToAdd.Photo))
        {
            return RepositoryResponse<Category>
                .CreateConflict("Invalid photo URL. " +
                                "The URL must be 255 characters or less and in a valid format. " +
                                "Please use a valid URL or a shortening service like bit.ly.");
        }

        return CreateEntity<Category, CategoryToUpsertDto>(categoryToAdd);
    }

    public RepositoryResponse<int> DeleteCategory(int categoryId)
    {
        return DeleteEntity<Category>(categoryId);
    }
}