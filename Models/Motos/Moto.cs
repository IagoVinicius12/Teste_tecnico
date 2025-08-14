using System;
using MongoDB.Entities;

namespace Models.MotoModel;

[Collection("Moto")]
public class Moto : Entity
{
    [Field("identifier")]
    public string Identifier { get; set; } = string.Empty;

    [Field("year")]
    public int Year { get; set; }

    [Field("model")]
    public string Model { get; set; } = string.Empty;

    [Field("plate")]
    public string Plate { get; set; } = string.Empty;

    [Field("islocated")]
    public bool IsLocated { get; set; } = false;

    public Moto() { }

    public Moto(string identifier, int year, string model, string plate)
    {
        Identifier = identifier;
        Year = year;
        Model = model;
        Plate = plate;
    }
}