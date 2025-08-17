namespace Models.Locacoes.Responses.DevolucaoLocacaoResponse;

public class DevolucaoLocacaoResponse
{
    public float TotalPrice { get; set; }

    public DateTime? DevolutionDate { get; set; }

    public DevolucaoLocacaoResponse()
    {
    }

    public DevolucaoLocacaoResponse(float totalPrice, DateTime devolutionDate)
    {
        TotalPrice = totalPrice;
        DevolutionDate = devolutionDate;
    }
}