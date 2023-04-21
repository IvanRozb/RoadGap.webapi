namespace Services.Abstractions;

public interface IServiceManager
{
    IUserService UserService { get; }
    ITagService TagService { get; }
    IProjectService ProjectService { get; }
    ITaskService TaskService { get; }
}