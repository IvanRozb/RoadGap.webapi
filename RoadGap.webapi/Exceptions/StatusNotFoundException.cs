using RoadGap.webapi.Exceptions.General;

namespace RoadGap.webapi.Exceptions;

public sealed class StatusNotFoundException : NotFoundException
{
    public StatusNotFoundException(int statusId)
        : base($"The status with the identifier {statusId} was not found.")    
    {
    }
}