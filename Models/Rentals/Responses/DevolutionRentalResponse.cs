namespace Models.Rentals.Responses.DevolutionRentalResponse;

public class DevolutionResponse
{
    public float TotalPrice { get; set; }

    public DateTime? DevolutionDate { get; set; }

    public DevolutionResponse()
    {
    }

    public DevolutionResponse(float totalPrice, DateTime devolutionDate)
    {
        TotalPrice = totalPrice;
        DevolutionDate = devolutionDate;
    }
}