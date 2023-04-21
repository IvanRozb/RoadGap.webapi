namespace Domain.Exceptions;

public sealed class TagNotFoundException : NotFoundException
{
    public TagNotFoundException(Guid tagId)
        : base($"The tag with the identifier {tagId} was not found."){}
}