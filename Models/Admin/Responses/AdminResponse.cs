namespace Models.Admin.Responses.AdminResponse;

public class AdminResponse
{
    public string id { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty;
    public AdminResponse() { }
    public AdminResponse(string id, string email, string name, string role)
    {
        this.id = id;
        this.email = email;
        this.name = name;
        this.role = role;
    }
}