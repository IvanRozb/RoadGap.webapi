using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories;

public interface ITaskRepository
{
    public void SaveChanges();
    public void AddEntity<T>(T entity);
    public void RemoveEntity<T>(T entity);
    public IEnumerable<TaskModel> GetTasks();
    public TaskModel? GetTaskById(int taskId);
    public IEnumerable<TaskModel> GetTasksBySearch(string searchParam);
    public bool CategoryExists(int categoryId);
    public bool StatusExists(int statusId);
    
}