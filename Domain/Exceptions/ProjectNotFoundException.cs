namespace Domain.Exceptions;

public sealed class ProjectNotFoundException : NotFoundException
{
    public ProjectNotFoundException(Guid projectId)
        : base($"The project with the identifier {projectId} was not found."){}
}