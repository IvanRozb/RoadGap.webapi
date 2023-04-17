using AutoMapper;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Helpers;
using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories.Implementation;

public class TaskRepository : Repository, ITaskRepository
{
    private readonly EntityChecker _entityChecker;
    public TaskRepository(IConfiguration configuration, IMapper mapper) 
        : base(configuration,mapper)
    {
        _entityChecker = new EntityChecker(EntityFramework);
    }

    public void Dispose()
    {
        EntityFramework.Tasks.RemoveRange(EntityFramework.Tasks);
        SaveChanges();
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
    
    public RepositoryResponse<TaskModel> EditTask(int taskId, TaskToUpsertDto taskDto)
    {
        try
        {
            if (!_entityChecker.CategoryExists(taskDto.CategoryId))
            {
                return new RepositoryResponse<TaskModel>
                {
                    Success = false,
                    Message = $"Category with ID {taskDto.CategoryId} not found",
                    StatusCode = 400
                };
            }
            
            if (!_entityChecker.StatusExists(taskDto.StatusId))
            {
                return new RepositoryResponse<TaskModel>
                {
                    Success = false,
                    Message = $"Status with ID {taskDto.StatusId} not found",
                    StatusCode = 400
                };
            }
            
            var task = GetTaskById(taskId);

            if (task == null)
            {
                return new RepositoryResponse<TaskModel>
                {
                    Success = false,
                    Message = $"Task with ID {taskId} not found",
                    StatusCode = 404
                };
            }
            
            Mapper.Map(taskDto, task);

            EntityFramework.SaveChanges();

            return new RepositoryResponse<TaskModel>
            {
                Success = true,
                Message = "Task updated successfully.",
                Data = task
            };
        }
        catch (Exception ex)
        {
            return new RepositoryResponse<TaskModel>
            {
                Success = false,
                Message = $"An error occurred while editing task with ID {taskId}: {ex.Message}",
                StatusCode = 500
            };
        }
    }

}