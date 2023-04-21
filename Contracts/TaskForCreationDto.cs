using System.ComponentModel.DataAnnotations;

namespace Contracts;

public class TaskForCreationDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters")]
    public string Title { get; set; } = "";
    
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    
    [Required]
    [Range(1,3)]
    public int Priority { get; set; }
    
    [Required]
    [Range(1,3)]
    public int Status { get; set; }
    
    
    [MaxLength(1000, ErrorMessage = "Tags can't be longer " +
                                    "than 1000 characters")]
    public string Tags { get; set; } = "";
}