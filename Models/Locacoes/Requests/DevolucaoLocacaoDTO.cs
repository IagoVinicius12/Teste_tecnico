using System.ComponentModel.DataAnnotations;


namespace Models.Locacoes.Requests.DevolucaoLocacaoDTO;

public class DevolucaoLocacaoDTO
{
    [Required (ErrorMessage = "Data Devolucao field is required")]
    public required DateTime? data_devolucao { get; set; }
}