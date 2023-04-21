using System.ComponentModel.DataAnnotations;

namespace Contracts;

public class ProjectForCreationDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters")]
    public string Title { get; set; } = "";
    
    public string? Description { get; set; }
}