using RoadGap.webapi.Dtos;
using RoadGap.webapi.Helpers;
using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories;

public interface ITaskRepository : IRepository
{
    public RepositoryResponse<IEnumerable<TaskModel>> GetTasks(string searchParam = "");
    public RepositoryResponse<TaskModel> GetTaskById(int taskId);
    public RepositoryResponse<TaskModel> EditTask(int taskId, TaskToUpsertDto taskDto);
    public RepositoryResponse<TaskModel> CreateTask(TaskToUpsertDto taskToAdd);
    public RepositoryResponse<int> DeleteTask(int taskId);
}