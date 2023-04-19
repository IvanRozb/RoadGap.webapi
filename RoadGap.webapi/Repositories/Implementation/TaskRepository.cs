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
        if (!EntityChecker.CategoryExists(taskDto.CategoryId))
        {
            return RepositoryResponse<TaskModel>.CreateConflict(
                $"Category with ID {taskDto.CategoryId} not found");
        }

        if (!EntityChecker.StatusExists(taskDto.StatusId))
        {
            return RepositoryResponse<TaskModel>.CreateConflict(
                $"Status with ID {taskDto.StatusId} not found");
        }
        
        return EditEntity(taskId, taskDto, GetTaskById, (dto, entity) => {
            Mapper.Map(dto, entity);
            entity.TaskUpdated = DateTime.Now;
        });
    }

    public RepositoryResponse<TaskModel> CreateTask(TaskToUpsertDto taskToAdd)
    {
        if (!EntityChecker.CategoryExists(taskToAdd.CategoryId))
        {
            return RepositoryResponse<TaskModel>
                .CreateConflict($"Category with ID {taskToAdd.CategoryId} not found");
        }

        if (!EntityChecker.StatusExists(taskToAdd.StatusId))
        {
            return RepositoryResponse<TaskModel>
                .CreateConflict($"Status with ID {taskToAdd.StatusId} not found");
        }

        return CreateEntity<TaskModel, TaskToUpsertDto>(taskToAdd);

    }

    public RepositoryResponse<int> DeleteTask(int taskId)
    {
        return DeleteEntity<Task>(taskId);
    }}