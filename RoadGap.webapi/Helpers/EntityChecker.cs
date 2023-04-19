using RoadGap.webapi.Data;

namespace RoadGap.webapi.Helpers;

public class EntityChecker
{
    private readonly DataContext _entityFramework;

    public EntityChecker(DataContext context)
    {
        _entityFramework = context;
    }
    
    public bool StatusExistsById(int statusId)
    {
        return _entityFramework.Status
            .Any(status => status.StatusId == statusId);
    }
    
    public bool StatusExistsByTitle(string statusTitle)
    {
        return _entityFramework.Status
            .Any(status => status.Title == statusTitle);
    }
    
    public bool CategoryExists(int categoryId)
    {
        return _entityFramework.Category
            .Any(category => category.CategoryId == categoryId);
    }
    
    public bool UserExistsWithEmail(string? email)
    {
        return _entityFramework.Users
            .Any(user => user.Email == email);
    }
    
    public bool UserExistsWithUserName(string? userName)
    {
        return _entityFramework.Users
            .Any(user => user.UserName == userName);
    }
}