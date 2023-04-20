namespace RoadGap.webapi.Exceptions.General;

public abstract class BadRequestException : Exception
{
    protected BadRequestException(string message)
        : base(message)
    {
        
    }
}