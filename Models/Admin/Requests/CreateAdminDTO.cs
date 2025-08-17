using System.ComponentModel.DataAnnotations;

namespace Models.Admin.Requests.CreateAdminDTO;

public class CreateAdminDTO
{
    [Required(ErrorMessage = "Email field is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string email { get; set; }

    [Required(ErrorMessage = "Password field is required")]
    [DataType(DataType.Password)]
    public required string senha { get; set; }

    [Required(ErrorMessage = "Name field is required")]
    public required string nome { get; set; }
}