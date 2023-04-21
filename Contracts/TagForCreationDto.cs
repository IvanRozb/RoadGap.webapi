using System.ComponentModel.DataAnnotations;

namespace Contracts;

public class TagForCreationDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
    public string Name { get; set; } = "";
    
    public string Description { get; set; } = "";
}