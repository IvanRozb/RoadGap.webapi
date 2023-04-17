using RoadGap.webapi.Data;

namespace RoadGap.webapi.Repositories.Implementation;

public abstract class Repository : IRepository
{
    protected readonly DataContext EntityFramework;

    protected Repository(IConfiguration configuration)
    {
        EntityFramework = new DataContext(configuration);
    }

    protected Repository(DataContext context)
    {
        EntityFramework = context;
    }
    public void SaveChanges()
    {
        if (EntityFramework.SaveChanges() < 0)
            throw new Exception("Failed to save changes to database");
    }

    public void AddEntity<T>(T entity)
    {
        if (entity == null) return;
        EntityFramework.Add(entity);
    }
    
    public void RemoveEntity<T>(T entity)
    {
        if (entity == null) return;
        EntityFramework.Remove(entity);
    }
}