using MongoDB.Entities;

namespace Models.EntregadorModel;

[Collection("Entregador")]
public class Entregador : Entity
{
    [Field("identifier")]
    public string Identifier { get; set; } = string.Empty;

    [Field("name")]
    public string Name { get; set; } = string.Empty;

    [Field("cnpj")]
    public string Cnpj { get; set; } = string.Empty;

    [Field("birthdate")]
    public DateTime Birthdate { get; set; } = DateTime.UtcNow;

    [Field("cnhNumber")]
    public string CnhNumber { get; set; } = string.Empty;

    [Field("cnhType")]
    public string CnhType { get; set; } = string.Empty;

    [Field("cnhImage")]
    public string CnhImage { get; set; } = string.Empty;

    public Entregador() { }

    public Entregador(string identifier, string name, string cnpj, DateTime birthdate, string cnhNumber, string cnhType, string cnhImage)
    {
        Identifier = identifier;
        Name = name;
        Cnpj = cnpj;
        Birthdate = birthdate;
        CnhNumber = cnhNumber;
        CnhType = cnhType;
        CnhImage = cnhImage;
    }
}
