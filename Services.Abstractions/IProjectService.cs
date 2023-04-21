using Contracts;

namespace Services.Abstractions;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<ProjectDto> GetByIdAsync(Guid projectId, CancellationToken cancellationToken = default);

    Task<ProjectDto> CreateAsync(ProjectForCreationDto projectForCreationDto, CancellationToken cancellationToken = default);

    Task UpdateAsync(Guid projectId, ProjectForUpdateDto projectForUpdateDto, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid projectId, CancellationToken cancellationToken = default);
}