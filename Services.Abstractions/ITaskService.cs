using Contracts;

namespace Services.Abstractions;

public interface ITaskService
{
    Task<IEnumerable<TaskDto>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<TaskDto>> GetAllByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);

    Task<TaskDto> GetByIdAsync(Guid taskId, CancellationToken cancellationToken = default);

    Task<TaskDto> CreateAsync(TaskForCreationDto taskForCreationDto, CancellationToken cancellationToken = default);

    Task UpdateAsync(Guid taskId, TaskForUpdateDto taskForUpdateDto, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid taskId, CancellationToken cancellationToken = default);
}