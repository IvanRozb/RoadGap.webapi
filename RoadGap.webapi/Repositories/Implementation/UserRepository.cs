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
        searchParam = searchParam.Trim();
        if (searchParam == "")
        {
            return RepositoryResponse<IEnumerable<User>>
                .CreateSuccess(EntityFramework.Users.ToList(),
                    "Users found successfully.");
        }

        var keywords = searchParam.ToLower().Split(' ');

        bool SearchPredicate(User user) => keywords.Any(keyword =>
            user.UserName.ToLower().Contains(keyword) || user.Email.ToLower().Contains(keyword));

        return SearchEntities((Func<User, bool>)SearchPredicate, "Users searched successfully.",
            "An error occurred while getting users.");
    }

    public RepositoryResponse<User> GetUserById(int userId)
    {
        return GetEntityById<User>(userId);
    }

    public RepositoryResponse<User> EditUser(int userId, UserToUpsertDto userDto)
    {
        // try
        // {
        //     if (EntityChecker.UserExistsWithEmail(userDto.Email))
        //     {
        //         return RepositoryResponse<User>.CreateConflict("The email is already taken!");
        //     }
        //     
        //     if (EntityChecker.UserExistsWithUserName(userDto.UserName))
        //     {
        //         return RepositoryResponse<User>.CreateConflict("The userName is already taken!");
        //     }
        //     
        //     var userResponse = GetUserById(userId);
        //
        //     if (!userResponse.Success)
        //     {
        //         return userResponse;
        //     }
        //
        //     var user = userResponse.Data;
        //     
        //     Mapper.Map(userDto, user);
        //
        //     EntityFramework.SaveChanges();
        //
        //     return RepositoryResponse<User>.CreateSuccess(user, "User updated successfully.");
        // }
        // catch (Exception ex)
        // {
        //     return RepositoryResponse<User>.CreateInternalServerError($"An error occurred while editing user with ID {userId}: {ex.Message}");
        // }
        if (EntityChecker.UserExistsWithEmail(userDto.Email))
        {
            return RepositoryResponse<User>.CreateConflict("The email is already taken!");
        }
            
        if (EntityChecker.UserExistsWithUserName(userDto.UserName))
        {
            return RepositoryResponse<User>.CreateConflict("The userName is already taken!");
        }
        
        return EditEntity(userId, userDto, GetUserById);
    }

    public RepositoryResponse<User> CreateUser(UserToUpsertDto userToAdd)
    {
        try
        {
            if (EntityChecker.UserExistsWithEmail(userToAdd.Email))
            {
                return RepositoryResponse<User>.CreateConflict("The email is already taken!");
            }
            
            if (EntityChecker.UserExistsWithUserName(userToAdd.UserName))
            {
                return RepositoryResponse<User>.CreateConflict("The userName is already taken!");
            }
            
            var user = Mapper.Map<User>(userToAdd);
            
            AddEntity(user);
            SaveChanges();

            return RepositoryResponse<User>.CreateSuccess(user, "User created successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<User>.CreateInternalServerError($"An error occurred while creating user: {ex.Message}");
        }
    }

    public RepositoryResponse<int> DeleteUser(int userId)
    {
        try
        {
            var response = GetUserById(userId);

            if (!response.Success)
            {
                return RepositoryResponse<int>.CreateBadRequest($"User with ID {userId} not found");
            }

            var user = response.Data;

            RemoveEntity(user);
            SaveChanges();

            return RepositoryResponse<int>.CreateSuccess(userId, "User deleted successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<int>.CreateInternalServerError($"An error occurred while editing user with ID {userId}: {ex.Message}");
        }
    }
}