using System;
using MongoDB.Entities;

namespace Models.RentalsModel;


[Collection("Rental")]
public class Rental : Entity
{
    public string Identifier { get; set; } = null!;

    public string DeliveryPersonId { get; set; } = null!;

    public string MotoId { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime DevolutionPrevisionDate { get; set; }

    public DateTime? DevolutionDate { get; set; }

    public int PlanType { get; set; }

    public float DailyPrice { get; set; }

    public float TotalPrice { get; set; }

    public Rental() { }

    public Rental(string identifier,string deliveryPersonId, string motoId, DateTime startDate, DateTime endDate, DateTime devolutionPrevisionDate, DateTime devolutionDate, int planType, float dailyPrice)
    {
        Identifier = identifier;
        DeliveryPersonId = deliveryPersonId;
        MotoId = motoId;
        StartDate = startDate;
        EndDate = endDate;
        DevolutionPrevisionDate = devolutionPrevisionDate;
        DevolutionDate = devolutionDate;
        PlanType = planType;
        DailyPrice = dailyPrice;
    }
}