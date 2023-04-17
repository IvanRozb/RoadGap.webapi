using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories;

public interface ITaskRepository : IRepository
{
    public IEnumerable<TaskModel> GetTasks();
    public TaskModel? GetTaskById(int taskId);
    public IEnumerable<TaskModel> GetTasksBySearch(string searchParam);
}