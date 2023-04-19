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

    public RepositoryResponse<TaskModel> EditTask(int taskId, TaskToUpsertDto taskToEdit)
    {
        if (!EntityChecker.CategoryExists(taskToEdit.CategoryId))
        {
            return RepositoryResponse<TaskModel>.CreateConflict(
                $"Category with ID {taskToEdit.CategoryId} does not exists");
        }

        if (!EntityChecker.StatusExistsById(taskToEdit.StatusId))
        {
            return RepositoryResponse<TaskModel>.CreateConflict(
                $"Status with ID {taskToEdit.StatusId} does not exists");
        }

        if (taskToEdit.Title.Length > 50)
        {
            return RepositoryResponse<TaskModel>
                .CreateConflict("Title must be 50 characters or less.");
        }

        if (taskToEdit.Description.Length > 1000)
        {
            return RepositoryResponse<TaskModel>
                .CreateConflict("Description must be 1000 characters or less.");
        }

        if (taskToEdit is { StartTime: { }, Deadline: { } }
            && taskToEdit.StartTime > taskToEdit.Deadline)
        {
            return RepositoryResponse<TaskModel>
                .CreateConflict("The start time cannot be later than the deadline.");
        }

        return EditEntity(taskId, taskToEdit, GetTaskById, (dto, entity) =>
        {
            Mapper.Map(dto, entity);
            entity.TaskUpdated = DateTime.Now;
        });
    }

    public RepositoryResponse<TaskModel> CreateTask(TaskToUpsertDto taskToAdd)
    {
        if (!EntityChecker.CategoryExists(taskToAdd.CategoryId))
        {
            return RepositoryResponse<TaskModel>.CreateConflict(
                $"Category with ID {taskToAdd.CategoryId} does not exists");
        }

        if (!EntityChecker.StatusExistsById(taskToAdd.StatusId))
        {
            return RepositoryResponse<TaskModel>.CreateConflict(
                $"Status with ID {taskToAdd.StatusId} does not exists");
        }

        if (taskToAdd.Title.Length > 50)
        {
            return RepositoryResponse<TaskModel>
                .CreateConflict("Title must be 50 characters or less.");
        }

        if (taskToAdd.Description.Length > 1000)
        {
            return RepositoryResponse<TaskModel>
                .CreateConflict("Description must be 1000 characters or less.");
        }

        if (taskToAdd is { StartTime: { }, Deadline: { } }
            && taskToAdd.StartTime > taskToAdd.Deadline)
        {
            return RepositoryResponse<TaskModel>
                .CreateConflict("The start time cannot be later than the deadline.");
        }

        return CreateEntity<TaskModel, TaskToUpsertDto>(taskToAdd, (dto) =>
        {
            var entity = Mapper.Map<TaskModel>(dto);
            entity.TaskUpdated = DateTime.Now;
            return entity;
        });
    }

    public RepositoryResponse<int> DeleteTask(int taskId)
    {
        return DeleteEntity<Task>(taskId);
    }
}