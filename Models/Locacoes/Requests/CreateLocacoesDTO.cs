using System.ComponentModel.DataAnnotations;

namespace Models.Locacoes.Requests.CreateLocacaoDTO;

public class CreateLocacaoDTO
{
	[Required(ErrorMessage = "Identifier field is required")]
	public required string identificador { get; set; }

	[Required(ErrorMessage = "Entregador ID field is required")]
	public required string entregador_id { get; set; }

	[Required(ErrorMessage = "Moto ID field is required")]
	public required string moto_id { get; set; }

	[Required(ErrorMessage = "Data de início field is required")]
	public required DateTime data_inicio { get; set; }

	[Required(ErrorMessage = "Data de fim field is required")]
	public required DateTime data_termino { get; set; }

	[Required(ErrorMessage="Data Previsao Termino field is required")]
	public required DateTime data_previsao_termino { get; set; }

	[Required(ErrorMessage = "Plano field is required")]
	[RegularExpression("^(7|15|30|45|50)$", ErrorMessage = "The plan needs to be 7, 15, 30, 45 or 50 days")]
	public int plano { get; set; }
}