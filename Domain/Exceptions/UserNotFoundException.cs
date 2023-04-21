namespace Domain.Exceptions;

public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(Guid userId)
        : base($"The user with the identifier {userId} was not found."){}
}