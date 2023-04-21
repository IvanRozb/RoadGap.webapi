using Contracts;

namespace Services.Abstractions;

public interface ITagService
{
    Task<IEnumerable<TagDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TagDto> CreateAsync(TagForCreationDto tagForCreationDto, CancellationToken cancellationToken = default);

    Task UpdateAsync(Guid tagId, TagForUpdateDto tagForUpdateDto, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid tagId, CancellationToken cancellationToken = default);
}