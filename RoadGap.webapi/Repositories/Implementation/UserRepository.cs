using AutoMapper;
using RoadGap.webapi.Data;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Helpers;
using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories.Implementation;

public class UserRepository : Repository, IUserRepository
{
    public UserRepository(IConfiguration configuration, IMapper mapper) 
        : base(configuration, mapper)
    {
    }

    public UserRepository(DataContext context, IMapper mapper) 
        : base(context, mapper)
    {
    }

    public RepositoryResponse<IEnumerable<User>> GetUsers(string searchParam = "")
    {
        try
        {
            searchParam = searchParam.Trim();
            if (searchParam == "")
            {
                return RepositoryResponse<IEnumerable<User>>
                    .CreateSuccess(EntityFramework.Users.ToList(),
                        "Users found successfully.");
            }

            var keywords = searchParam.ToLower().Split(' ');
            var searchedUsers = EntityFramework.Users.AsEnumerable()
                .Where(user => keywords.Any(keyword =>
                    user.UserName.ToLower().Contains(keyword) ||
                    user.Email.ToLower().Contains(keyword)))
                .ToList();

            return RepositoryResponse<IEnumerable<User>>
                .CreateSuccess(searchedUsers,
                    "Users searched successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<IEnumerable<User>>
                .CreateInternalServerError($"An error occurred while getting users: {ex.Message}");
        }
    }

    public RepositoryResponse<User> GetUserById(int userId)
    {
        throw new NotImplementedException();
    }

    public RepositoryResponse<User> EditUser(int userId, UserToUpsertDto userDto)
    {
        throw new NotImplementedException();
    }

    public RepositoryResponse<User> CreateUser(UserToUpsertDto userToAdd)
    {
        throw new NotImplementedException();
    }

    public RepositoryResponse<int> DeleteUser(int userId)
    {
        throw new NotImplementedException();
    }
}