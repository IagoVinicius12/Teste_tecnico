namespace Models.Motos.Responses.MotoResponse;

public class MotoResponse
{
    public string Id { get; set; } = string.Empty;

    public string Identifier { get; set; } = string.Empty;

    public int Year { get; set; }

    public string Model { get; set; } = string.Empty;

    public string Plate { get; set; } = string.Empty;

    public bool IsLocated { get; set; } = false;

    public MotoResponse() { }
    public MotoResponse(string id, string identifier, int year, string model, string plate, bool isLocated)
    {
        Id = id;
        Identifier = identifier;
        Year = year;
        Model = model;
        Plate = plate;
        IsLocated = isLocated;
    }
}