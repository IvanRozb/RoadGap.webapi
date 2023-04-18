using RoadGap.webapi.Dtos;
using RoadGap.webapi.Helpers;
using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories;

public interface ICategoryRepository : IRepository
{
    public RepositoryResponse<IEnumerable<Category>> GetCategories(string searchParam = "");
    public RepositoryResponse<Category> GetCategoryById(int categoryId);
    public RepositoryResponse<Category> EditCategory(int categoryId, CategoryToUpsertDto categoryDto);
    public RepositoryResponse<Category> CreateCategory(CategoryToUpsertDto categoryToAdd);
    public RepositoryResponse<int> DeleteCategory(int categoryId);
}