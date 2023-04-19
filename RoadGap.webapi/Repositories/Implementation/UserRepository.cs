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
        if (!Validator.ValidateUserName(userToAdd.UserName))
        {
            return RepositoryResponse<User>.CreateConflict(
                "Invalid user name. User name should only contain alphabets, " +
                "numbers and must contain less or equal than 50 characters.");
        }

        if (EntityChecker.UserExistsWithUserName(userToAdd.UserName))
        {
            return RepositoryResponse<User>
                .CreateConflict("The user with this userName already exists.");
        }

        if (!Validator.ValidateEmail(userToAdd.Email))
        {
            return RepositoryResponse<User>.CreateConflict(
                "Invalid email format. Email must look like \"test@test.com\" " +
                "and contain less or equal than 320 characters");
        }

        if (EntityChecker.UserExistsWithEmail(userToAdd.Email))
        {
            return RepositoryResponse<User>
                .CreateConflict("The user with this email already exists.");
        }

        return CreateEntity<User, UserToUpsertDto>(userToAdd);
    }

    public RepositoryResponse<int> DeleteUser(int userId)
    {
        return DeleteEntity<User>(userId);
    }}