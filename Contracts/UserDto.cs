using Domain.Entities;

namespace Contracts;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IEnumerable<TaskModel> Tasks { get; set; }
    public IEnumerable<Project> Projects { get; set; }
}