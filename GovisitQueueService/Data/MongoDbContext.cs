using GovisitQueueService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GovisitQueueService.Data;

/// <summary>Resolves the configured database and exposes the typed collections.</summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly string _appointmentsCollectionName;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
        _appointmentsCollectionName = settings.Value.AppointmentsCollectionName;
    }

    public IMongoCollection<Appointment> Appointments =>
        _database.GetCollection<Appointment>(_appointmentsCollectionName);
}
