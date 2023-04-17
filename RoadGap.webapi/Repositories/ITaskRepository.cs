using RoadGap.webapi.Dtos;
using RoadGap.webapi.Helpers;
using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories;

public interface ITaskRepository : IRepository
{
    public IEnumerable<TaskModel> GetTasks();
    public RepositoryResponse<TaskModel> GetTaskById(int taskId);
    public IEnumerable<TaskModel> GetTasksBySearch(string searchParam);
    public RepositoryResponse<TaskModel> EditTask(int taskId, TaskToUpsertDto taskDto);
}