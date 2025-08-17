namespace Models.Locacoes.Responses.LocacaoResponse;

public class LocacaoResponse
{
	public string id { get; set; } = string.Empty;

	public string identificador { get; set; } = string.Empty;

	public string entregador_id { get; set; } = string.Empty;

	public string moto_id { get; set; } = string.Empty;

	public DateTime data_inicio { get; set; } = DateTime.UtcNow;

	public DateTime data_termino { get; set; } = DateTime.UtcNow;

	public DateTime data_previsao_termino { get; set; } = DateTime.UtcNow;

	public float dailyPrice { get; set; } = 0.0f;	

    public int plano { get; set; }

	public float totalPrice { get; set; } = 0.0f;

    public LocacaoResponse() { }

	public LocacaoResponse(string id, string identificador, string entregador_id, string moto_id, DateTime data_inicio, DateTime data_termino, DateTime data_previsao_termino,float dailyPrice, int plano, float totalPrice)
	{
		this.id = id;
		this.identificador = identificador;
		this.entregador_id = entregador_id;
		this.moto_id = moto_id;
		this.data_inicio = data_inicio;
		this.data_termino = data_termino;
		this.data_previsao_termino = data_previsao_termino;
		this.dailyPrice = dailyPrice;
        this.plano = plano;
		this.totalPrice = totalPrice;
    }
}