using GovisitQueueService.Models;
using MongoDB.Driver;

namespace GovisitQueueService.Data;

/// <summary>
/// Loads the fixed sample appointments declared in configuration. Invoked from
/// Program.cs for the Development environment only.
/// </summary>
public static class DataSeeder
{
    public static async Task SeedAsync(MongoDbContext context, IConfiguration configuration)
    {
        var seedData = new SeedDataOptions();
        configuration.GetSection("SeedData").Bind(seedData);

        if (seedData.Appointments.Count == 0)
        {
            return;
        }

        // Skip when the collection already holds documents, so repeated runs against a
        // persistent database do not accumulate duplicates.
        var existingCount = await context.Appointments.CountDocumentsAsync(FilterDefinition<Appointment>.Empty);
        if (existingCount > 0)
        {
            return;
        }

        var appointments = seedData.Appointments.Select(a => new Appointment
        {
            CustomerName = a.CustomerName,
            CustomerPhone = a.CustomerPhone,
            ServiceType = a.ServiceType,
            AppointmentDate = a.AppointmentDate,
            Status = string.IsNullOrWhiteSpace(a.Status) ? AppointmentStatuses.Scheduled : a.Status,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        await context.Appointments.InsertManyAsync(appointments);
    }
}
