using System.ComponentModel.DataAnnotations;

namespace Models.Motos.Requests.UpdateMotoPlateDTO;

public class UpdateMotoPlateDTO
{
	[Required(ErrorMessage = "Identifier field is required")]
	public required string identifier { get; set; }

	[Required(ErrorMessage = "Plate field is required")]
	[RegularExpression(@"^[A-Z]{3}-\d{4}$", ErrorMessage = "Plate must be in the format 'AAA-1234'")]
	public required string plate { get; set; }
}
