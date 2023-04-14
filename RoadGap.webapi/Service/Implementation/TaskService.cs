using RoadGap.webapi.Data;
using Task = RoadGap.webapi.Models.Task;

namespace RoadGap.webapi.Service.Implementation;

public class TaskService : ITaskService
{
    private readonly DataContext _entityFramework;

    public TaskService(IConfiguration configuration)
    {
        _entityFramework = new DataContext(configuration);
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
    
    public IEnumerable<Task> GetTasks() =>
        _entityFramework.Tasks.ToList();

    public Task? GetTaskById(int taskId)
    {
        var task = _entityFramework.Tasks
            .FirstOrDefault(task => task.TaskId == taskId);
        
        return task;
    }

    public IEnumerable<Task> GetTasksBySearch(string searchParam)
    {
        var keywords = searchParam.Split(' ');
        var tasks = _entityFramework.Tasks;
        var searchedTasks = new List<Task>();

        foreach (var keyword in keywords)
        {
            searchedTasks.AddRange(tasks.Where(task =>
                    task.Title.Contains(keyword) ||
                    task.Description.Contains(keyword))
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