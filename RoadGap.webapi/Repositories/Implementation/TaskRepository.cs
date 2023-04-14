using RoadGap.webapi.Data;
using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories.Implementation;

public class TaskRepository : ITaskRepository, IDisposable
{
    private readonly DataContext _entityFramework;

    public TaskRepository(IConfiguration configuration)
    {
        _entityFramework = new DataContext(configuration);
    }

    public TaskRepository(DataContext context)
    {
        _entityFramework = context;
    }
    public void Dispose()
    {
        _entityFramework.Tasks.RemoveRange(_entityFramework.Tasks);
        SaveChanges();
    }


    public void SaveChanges()
    {
        if (_entityFramework.SaveChanges() < 0)
            throw new Exception("Failed to save changes to database");
    }

    public void AddEntity<T>(T entity)
    {
        if (entity == null) return;
        _entityFramework.Add(entity);
    }
    
    public void RemoveEntity<T>(T entity)
    {
        if (entity == null) return;
        _entityFramework.Remove(entity);
    }
    
    public IEnumerable<TaskModel> GetTasks() =>
        _entityFramework.Tasks.ToList();

    public TaskModel? GetTaskById(int taskId)
    {
        var task = _entityFramework.Tasks
            .FirstOrDefault(task => task.TaskId == taskId);
        
        return task;
    }

    public IEnumerable<TaskModel> GetTasksBySearch(string searchParam)
    {
        var keywords = searchParam.ToLower().Split(' ');
        var tasks = _entityFramework.Tasks;
        var searchedTasks = new List<TaskModel>();
        
        foreach (var keyword in keywords)
        {
            searchedTasks.AddRange(tasks.Where(task =>
                    task.Title.ToLower().Contains(keyword) ||
                    task.Description.ToLower().Contains(keyword))
                .ToList());
        }

        return searchedTasks;
    }

    public bool CategoryExists(int categoryId)
    {
        return _entityFramework.Category
            .Any(category => category.CategoryId == categoryId);
    }
    
    public bool StatusExists(int statusId)
    {
        return _entityFramework.Status
            .Any(status => status.StatusId == statusId);
    }
}