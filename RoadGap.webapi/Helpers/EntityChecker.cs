using RoadGap.webapi.Data;

namespace RoadGap.webapi.Helpers;

public class EntityChecker
{
    private readonly DataContext _entityFramework;

    public EntityChecker(DataContext context)
    {
        _entityFramework = context;
    }
    
    public bool StatusExists(int statusId)
    {
        return _entityFramework.Status
            .Any(status => status.StatusId == statusId);
    }

    public bool CategoryExists(int categoryId)
    {
        return _entityFramework.Category
            .Any(status => status.CategoryId == categoryId);
    }
}