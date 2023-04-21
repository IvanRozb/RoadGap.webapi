using Domain.Entities;

namespace Domain.Repositories;

public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Tag> GetByIdAsync(Guid projectId, CancellationToken cancellationToken = default);

    void Insert(Tag project);

    void Remove(Tag project);
}