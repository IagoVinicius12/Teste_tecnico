namespace Models.Entregadores.Responses.EntregadorResponse;
public class EntregadorResponse
{
	public string id { get; set; } =string.Empty;
    public string identifier { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public string cnpj { get; set; } =string.Empty;
    public string cnh { get; set; } = string.Empty;
    public string cnhType { get; set; } = string.Empty;
    public DateTime birthdate { get; set; } = DateTime.UtcNow;  

    public EntregadorResponse() { }

    public EntregadorResponse(string id, string identifier,string name, string cnpj, string cnh,string cnhtype, DateTime birthdate)
    {
        this.id = id;
        this.identifier = identifier;
        this.name = name;
        this.cnpj = cnpj;
        this.cnh = cnh;
        this.cnhType = cnhtype;
        this.birthdate = birthdate;
    }
}