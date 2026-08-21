namespace GovisitQueueService.Data;

/// <summary>Shape of the MongoDbSettings section in appsettings.json.</summary>
public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string AppointmentsCollectionName { get; set; } = string.Empty;
}
