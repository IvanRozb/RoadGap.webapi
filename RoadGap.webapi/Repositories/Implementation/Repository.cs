using AutoMapper;
using RoadGap.webapi.Data;
using RoadGap.webapi.Helpers;

namespace RoadGap.webapi.Repositories.Implementation;

public abstract class Repository : IRepository
{
    protected readonly DataContext EntityFramework;
    protected readonly IMapper Mapper;
    protected readonly EntityChecker EntityChecker;

    protected Repository(IConfiguration configuration, IMapper mapper)
    {
        EntityFramework = new DataContext(configuration);
        Mapper = mapper;
        EntityChecker = new EntityChecker(EntityFramework);
    }
    protected Repository(DataContext context, IMapper mapper)
    {
        EntityFramework = context;
        Mapper = mapper;
        EntityChecker = new EntityChecker(EntityFramework);
    }
    
    public EntityChecker GetEntityChecker()
    {
        return EntityChecker;
    }

    public DataContext GetDataContext()
    {
        return EntityFramework;
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