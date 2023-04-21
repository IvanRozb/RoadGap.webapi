using Domain.Entities;

namespace Domain.Repositories;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<Project> GetByIdAsync(Guid projectId, CancellationToken cancellationToken = default);

    void Insert(Project project);

    void Remove(Project project);
}