using System.ComponentModel.DataAnnotations;

namespace Contracts;

public class UserForCreationDto
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username can't be longer than 50 characters")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(320, ErrorMessage = "Email can't be longer than 320 characters")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters")]
    public string FirstName { get; set; } = "";

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters")]
    public string LastName { get; set; } = "";
}