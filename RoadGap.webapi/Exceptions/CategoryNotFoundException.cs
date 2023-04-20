using RoadGap.webapi.Exceptions.General;

namespace RoadGap.webapi.Exceptions;

public sealed class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException(int categoryId)
        : base($"The category with the identifier {categoryId} was not found.")    
    {
    }
}