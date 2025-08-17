using System.ComponentModel.DataAnnotations;

namespace Models.Motos.Requests.CreateMotoDTO;

public class CreateMotoDTO
{
    [Required(ErrorMessage = "Identifier field is required")]
    public required string identifier { get; set; }

    [Required(ErrorMessage = "Year field is required")]
    [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
    public required int year { get; set; }

    [Required(ErrorMessage = "Model field is required")]
    public required string model { get; set; }

    [Required(ErrorMessage = "Plate field is required")]
    [RegularExpression(@"^[A-Z]{3}-\d{4}$", ErrorMessage = "Plate must be in the format 'AAA-1234'")]
    public required string plate { get; set; }
}