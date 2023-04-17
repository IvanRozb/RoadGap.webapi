using RoadGap.webapi.Data;
using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories.Implementation;

public class TaskRepository : Repository, ITaskRepository
{
    public TaskRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public TaskRepository(DataContext context) : base(context)
    {
    }
    public void Dispose()
    {
        EntityFramework.Tasks.RemoveRange(EntityFramework.Tasks);
        SaveChanges();
    }

    public DataContext GetDataContext()
    {
        return EntityFramework;
    }
    
    public IEnumerable<TaskModel> GetTasks() =>
        EntityFramework.Tasks.ToList();

    public TaskModel? GetTaskById(int taskId)
    {
        var task = EntityFramework.Tasks
            .FirstOrDefault(task => task.TaskId == taskId);
        
        return task;
    }

    public IEnumerable<TaskModel> GetTasksBySearch(string searchParam)
    {
        var keywords = searchParam.ToLower().Split(' ');
        var tasks = EntityFramework.Tasks;
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
}