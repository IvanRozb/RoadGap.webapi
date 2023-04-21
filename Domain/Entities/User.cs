using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User
{
    [Required]
    public Guid Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<Task> Tasks { get; set; }
    public ICollection<Project> Projects { get; set; }
}