using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GovisitQueueService.Models;

public class Appointment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    [BsonElement("customerPhone")]
    public string CustomerPhone { get; set; } = string.Empty;

    [BsonElement("serviceType")]
    public string ServiceType { get; set; } = string.Empty;

    [BsonElement("appointmentDate")]
    public DateTime AppointmentDate { get; set; }

    [BsonElement("status")]
    public string Status { get; set; } = AppointmentStatuses.Scheduled;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public static class AppointmentStatuses
{
    public const string Scheduled = "Scheduled";
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";
}
