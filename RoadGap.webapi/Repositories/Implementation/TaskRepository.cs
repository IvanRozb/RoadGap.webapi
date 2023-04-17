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

    public RepositoryResponse<IEnumerable<TaskModel>> GetTasks(string searchParam = "") {
        try
        {
            if (searchParam == "")
            {
                RepositoryResponse<IEnumerable<TaskModel>>
                    .CreateSuccess(EntityFramework.Tasks.ToList(),
                        "Tasks found successfully.");
            }

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

            return RepositoryResponse<IEnumerable<TaskModel>>
                .CreateSuccess(searchedTasks,
                    "Tasks searched successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<IEnumerable<TaskModel>>
                .CreateInternalServerError($"An error occurred while getting tasks: {ex.Message}");
        }
    }

    public RepositoryResponse<TaskModel> GetTaskById(int taskId)
    {
        var task = EntityFramework.Tasks
            .FirstOrDefault(task => task.TaskId == taskId);
        
        if (task == null)
        {
            return RepositoryResponse<TaskModel>.CreateNotFound($"Task with ID {taskId} not found");
        }
        
        return RepositoryResponse<TaskModel>
            .CreateSuccess(task, "Task found successfully.");
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
                return RepositoryResponse<TaskModel>.CreateBadRequest($"Category with ID {taskDto.CategoryId} not found");
            }
            
            if (!_entityChecker.StatusExists(taskDto.StatusId))
            {
                return RepositoryResponse<TaskModel>.CreateBadRequest($"Status with ID {taskDto.StatusId} not found");
            }
            
            var taskResponse = GetTaskById(taskId);

            if (!taskResponse.Success)
            {
                return taskResponse;
            }

            var task = taskResponse.Data;
            
            Mapper.Map(taskDto, task);
            task.TaskUpdated = DateTime.Now;

            EntityFramework.SaveChanges();

            return RepositoryResponse<TaskModel>.CreateSuccess(task, "Task updated successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<TaskModel>.CreateInternalServerError($"An error occurred while editing task with ID {taskId}: {ex.Message}");
        }
    }

    public RepositoryResponse<TaskModel> CreateTask(TaskToUpsertDto taskToAdd)
    {
        try
        {
            if (!_entityChecker.CategoryExists(taskToAdd.CategoryId))
            {
                return RepositoryResponse<TaskModel>.CreateBadRequest($"Category with ID {taskToAdd.CategoryId} not found");
            }
            
            if (!_entityChecker.StatusExists(taskToAdd.StatusId))
            {
                return RepositoryResponse<TaskModel>.CreateBadRequest($"Status with ID {taskToAdd.StatusId} not found");
            }

            var task = Mapper.Map<TaskModel>(taskToAdd);
            task.TaskUpdated = DateTime.Now;
            
            AddEntity(task);
            SaveChanges();

            return RepositoryResponse<TaskModel>.CreateSuccess(task, "Task created successfully.");
        }
        catch (Exception ex)
        {
            return RepositoryResponse<TaskModel>.CreateInternalServerError($"An error occurred while creating task: {ex.Message}");
        }
    }

    public RepositoryResponse<int> DeleteTask(int taskId)
    {
        var response = GetTaskById(taskId);
        
        if (!response.Success)
        {
            return RepositoryResponse<int>.CreateBadRequest($"Task with ID {taskId} not found");
        }

        var task = response.Data;

        RemoveEntity(task);
        SaveChanges();
        
        return RepositoryResponse<int>.CreateSuccess(taskId, "Task deleted successfully.");
    }
}