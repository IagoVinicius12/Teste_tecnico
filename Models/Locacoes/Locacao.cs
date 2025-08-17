using System;
using MongoDB.Entities;

namespace Models.LocacoesModel;


[Collection("Locacao")]
public class Locacao : Entity
{
    public string Identifier { get; set; } = null!;

    public string EntregadorId { get; set; } = null!;

    public string MotoId { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime DevolutionPrevisionDate { get; set; }

    public DateTime? DevolutionDate { get; set; }

    public int PlanType { get; set; }

    public float DailyPrice { get; set; }

    public float TotalPrice { get; set; }

    public Locacao() { }

    public Locacao(string identifier,string entregadorId, string motoId, DateTime startDate, DateTime endDate, DateTime devolutionPrevisionDate, DateTime devolutionDate, int planType, float dailyPrice)
    {
        Identifier = identifier;
        EntregadorId = entregadorId;
        MotoId = motoId;
        StartDate = startDate;
        EndDate = endDate;
        DevolutionPrevisionDate = devolutionPrevisionDate;
        DevolutionDate = devolutionDate;
        PlanType = planType;
        DailyPrice = dailyPrice;
    }
}