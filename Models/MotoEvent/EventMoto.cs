using MongoDB.Entities;
namespace Models.EventMotoModel;


[Collection("EventMoto")]
public class EventMoto: Entity
{
    public string Identifier { get; set; } = string.Empty;

    public string EventType { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }

    public string MotoId { get; set; } = string.Empty;

    public EventMoto() { }

    public EventMoto(string identifier, string eventType, string description, DateTime eventDate, string motoId)
    {
        Identifier = identifier;
        EventType = eventType;
        Description = description;
        EventDate = eventDate;
        MotoId = motoId;
    }
}