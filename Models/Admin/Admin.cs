using MongoDB.Entities;
namespace Models.AdminModel;

[Collection("Admin")]
public class Admin : Entity
{
    [Field("email")]
    public string Email { get; set; } = string.Empty;

    [Field("password")]
    public string Password { get; set; } = string.Empty;

    [Field("name")]
    public string Name { get; set; } = string.Empty;

    [Field("role")]
    public string Role { get; set; } = "Admin";
    public Admin() { }
    public Admin(string email, string password, string name)
    {
        Email = email;
        Password = password;
        Name = name;
    }
}