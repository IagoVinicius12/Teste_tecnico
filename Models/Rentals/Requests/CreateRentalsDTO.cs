using System.ComponentModel.DataAnnotations;

namespace Models.Rentals.Requests.CreateRentalDTO;

public class CreateRentalDTO
{
	[Required(ErrorMessage = "Identifier field is required")]
	public required string identifier { get; set; }

	[Required(ErrorMessage = "Delivery Person Identifier field is required")]
	public required string deliveryPerson_Identifier { get; set; }

	[Required(ErrorMessage = "Moto ID field is required")]
	public required string moto_id { get; set; }

	[Required(ErrorMessage = "Start Date field is required")]
	public required DateTime start_date { get; set; }

	[Required(ErrorMessage = "End Date field is required")]
	public required DateTime end_date { get; set; }

	[Required(ErrorMessage="Prevision Devolution Date field is required")]
	public required DateTime prevision_devolution_date { get; set; }

	[Required(ErrorMessage = "Plan field is required")]
	[RegularExpression("^(7|15|30|45|50)$", ErrorMessage = "The plan needs to be 7, 15, 30, 45 or 50 days")]
	public int plan { get; set; }
}