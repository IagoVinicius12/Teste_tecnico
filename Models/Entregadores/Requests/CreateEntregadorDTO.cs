using System.ComponentModel.DataAnnotations;
namespace Models.Entregadores.Requests.CreateEntregadorDTO;

public class CreateEntregadorDTO
{
    [Required(ErrorMessage = "Identifier field is obrigatory")]
    public required string identificador { get; set; }

    [Required(ErrorMessage="Name field is obrigatory")]
    public required string nome { get; set; }

    [Required (ErrorMessage = "cnpj field is obrigatory")]
    public required string cnpj { get; set; }

    [Required(ErrorMessage = "birthdate field is obrigatory")]
    public required DateTime data_nascimento { get; set; }

    [Required(ErrorMessage = "cnhnumber field is obrigatory")]
    public required string numero_cnh { get; set; }

    [Required(ErrorMessage = "cnhtype field is obrigatory")]
    [RegularExpression("^(A|B|A\\+B)$", ErrorMessage = "O tipo de CNH deve ser 'A', 'B' ou 'A+B'.")]
    public required string tipo_cnh { get; set; }
}