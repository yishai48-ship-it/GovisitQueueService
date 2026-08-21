namespace GovisitQueueService.Data;

/// <summary>Shape of the SeedData section in appsettings.Development.json.</summary>
public class SeedDataOptions
{
    public List<SeedAppointmentOptions> Appointments { get; set; } = new();
}

public class SeedAppointmentOptions
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
