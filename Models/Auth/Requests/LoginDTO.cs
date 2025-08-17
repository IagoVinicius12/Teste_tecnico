using System.ComponentModel.DataAnnotations;

namespace Models.Auth.Requests.LoginDTO;

public class LoginDTO
{
    [Required(ErrorMessage = "Email field is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password field is required")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

}