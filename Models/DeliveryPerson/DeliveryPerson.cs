using MongoDB.Entities;

namespace Models.DeliveryPersonModel;

[Collection("DeliveryPerson")]
public class DeliveryPerson : Entity
{
    [Field("identifier")]
    public string Identifier { get; set; } = string.Empty;

    [Field("email")]
    public string Email { get; set; } = string.Empty;

    [Field("password")]
    public string Password { get; set; } = string.Empty;

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
    public string CnhImagepath { get; set; } = string.Empty;// so salva o caminho do base64 da imagem

    [Field("role")]
    public string Role { get; set; } = "DeliveryPerson";

    public DeliveryPerson() { }

    public DeliveryPerson(string identifier,string email, string password, string name, string cnpj, DateTime birthdate, string cnhNumber, string cnhType, string cnhImage)
    {
        Identifier = identifier;
        Email = email;
        Password = password;
        Name = name;
        Cnpj = cnpj;
        Birthdate = birthdate;
        CnhNumber = cnhNumber;
        CnhType = cnhType;
        CnhImagepath = cnhImage;
    }
}
