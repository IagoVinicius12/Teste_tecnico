namespace Models.Rentals.Responses.RentalResponse;

public class RentalResponse
{
	public string id { get; set; } = string.Empty;

	public string identifier { get; set; } = string.Empty;

	public string delivery_personId { get; set; } = string.Empty;

	public string moto_id { get; set; } = string.Empty;

	public DateTime start_date { get; set; } = DateTime.UtcNow;

	public DateTime end_date { get; set; } = DateTime.UtcNow;

	public DateTime prevision_devolution_date { get; set; } = DateTime.UtcNow;

	public float dailyPrice { get; set; } = 0.0f;	

    public int plan { get; set; }

	public float totalPrice { get; set; } = 0.0f;

    public RentalResponse() { }

	public RentalResponse(string id, string identifier, string delivery_personId, string moto_id, DateTime start_date, DateTime end_date, DateTime prevision_devolution_date,float dailyPrice, int plan, float totalPrice)
	{
		this.id = id;
		this.identifier = identifier;
		this.delivery_personId = delivery_personId;
		this.moto_id = moto_id;
		this.start_date = start_date;
		this.end_date = end_date;
		this.prevision_devolution_date = prevision_devolution_date;
		this.dailyPrice = dailyPrice;
        this.plan = plan;
		this.totalPrice = totalPrice;
    }
}