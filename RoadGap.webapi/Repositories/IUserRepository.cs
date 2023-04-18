using RoadGap.webapi.Dtos;
using RoadGap.webapi.Helpers;
using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories;

public interface IUserRepository
{
    public RepositoryResponse<IEnumerable<User>> GetUsers(string searchParam = "");
    public RepositoryResponse<User> GetUserById(int userId);
    public RepositoryResponse<User> EditUser(int userId, UserToUpsertDto userDto);
    public RepositoryResponse<User> CreateUser(UserToUpsertDto userToAdd);
    public RepositoryResponse<int> DeleteUser(int userId);
}