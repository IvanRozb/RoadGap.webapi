namespace Domain.Exceptions;

public sealed class TaskNotFoundException : NotFoundException
{
    public TaskNotFoundException(Guid taskId)
        : base($"The task with the identifier {taskId} was not found."){}
}