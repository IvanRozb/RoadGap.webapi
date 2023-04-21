using Domain.Entities;

namespace Domain.Repositories;

public interface ITaskModelRepository
{
    Task<IEnumerable<TaskModel>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TaskModel>> GetAllByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);

    Task<TaskModel> GetByIdAsync(Guid projectId, CancellationToken cancellationToken = default);

    void Insert(TaskModel project);

    void Remove(TaskModel project);
}