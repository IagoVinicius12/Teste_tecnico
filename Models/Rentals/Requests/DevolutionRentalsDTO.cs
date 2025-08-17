using System.ComponentModel.DataAnnotations;


namespace Models.Rentals.Requests.DevolutionRentalDTO;

public class DevolutionRentalDTO
{
    [Required (ErrorMessage = "Devolution date field is required")]
    public required DateTime? devolution_date { get; set; }
}