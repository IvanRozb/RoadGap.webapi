using Task = RoadGap.webapi.Models.Task;

namespace RoadGap.webapi.Repositories;

public interface ITaskRepository
{
    public void SaveChanges();
    public void AddEntity<T>(T entity);
    public void RemoveEntity<T>(T entity);
    public IEnumerable<Task> GetTasks();
    public Task? GetTaskById(int taskId);
    public IEnumerable<Task> GetTasksBySearch(string searchParam);
    public bool CategoryExists(int categoryId);
    public bool StatusExists(int statusId);
    
}