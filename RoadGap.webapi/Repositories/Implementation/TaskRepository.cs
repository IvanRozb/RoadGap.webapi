using AutoMapper;
using RoadGap.webapi.Data;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Helpers;
using RoadGap.webapi.Models;

namespace RoadGap.webapi.Repositories.Implementation;

public class TaskRepository : Repository, ITaskRepository
{
    public TaskRepository(IConfiguration configuration, IMapper mapper)
        : base(configuration, mapper)
    {
    }

    public TaskRepository(DataContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public void Dispose()
    {
        EntityFramework.Tasks.RemoveRange(EntityFramework.Tasks);
        SaveChanges();
    }

    public RepositoryResponse<IEnumerable<TaskModel>> GetTasks(string searchParam = "")
    {
        searchParam = searchParam.Trim();
        if (searchParam == "")
        {
            return RepositoryResponse<IEnumerable<TaskModel>>
                .CreateSuccess(EntityFramework.Tasks.ToList(),
                    "Tasks found successfully.");
        }

        var keywords = searchParam.ToLower().Split(' ');

        bool SearchPredicate(TaskModel task) => keywords.Any(keyword =>
            task.Title.ToLower().Contains(keyword) || task.Description.ToLower().Contains(keyword));

        return SearchEntities((Func<TaskModel, bool>)SearchPredicate, "Tasks searched successfully.",
            "An error occurred while getting tasks.");
    }

    public RepositoryResponse<TaskModel> GetTaskById(int taskId)
    {
        return GetEntityById<TaskModel>(taskId);
    }

    public RepositoryResponse<TaskModel> EditTask(int taskId, TaskToUpsertDto taskDto)
    {
        try
        {
            if (!EntityChecker.CategoryExists(taskDto.CategoryId))
            {
                return RepositoryResponse<TaskModel>.CreateBadRequest(
                    $"Category with ID {taskDto.CategoryId} not found");
            }

            if (!EntityChecker.StatusExists(taskDto.StatusId))
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
            return RepositoryResponse<TaskModel>.CreateInternalServerError(
                $"An error occurred while editing task with ID {taskId}: {ex.Message}");
        }
    }

    public RepositoryResponse<TaskModel> CreateTask(TaskToUpsertDto taskToAdd)
    {
        try
        {
            if (!EntityChecker.CategoryExists(taskToAdd.CategoryId))
            {
                return RepositoryResponse<TaskModel>.CreateBadRequest(
                    $"Category with ID {taskToAdd.CategoryId} not found");
            }

            if (!EntityChecker.StatusExists(taskToAdd.StatusId))
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
            return RepositoryResponse<TaskModel>.CreateInternalServerError(
                $"An error occurred while creating task: {ex.Message}");
        }
    }

    public RepositoryResponse<int> DeleteTask(int taskId)
    {
        try
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
        catch (Exception ex)
        {
            return RepositoryResponse<int>.CreateInternalServerError(
                $"An error occurred while editing task with ID {taskId}: {ex.Message}");
        }
    }
}