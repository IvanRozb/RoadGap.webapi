using RoadGap.webapi.Exceptions.General;

namespace RoadGap.webapi.Exceptions;

public sealed class TaskNotFoundException : NotFoundException
{
    public TaskNotFoundException(int taskId)
        : base($"The task with the identifier {taskId} was not found.")    
    {
    }
}