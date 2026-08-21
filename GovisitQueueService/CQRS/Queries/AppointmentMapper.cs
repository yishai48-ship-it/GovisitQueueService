using GovisitQueueService.DTOs;
using GovisitQueueService.Models;

namespace GovisitQueueService.CQRS.Queries;

/// <summary>Single conversion point from the Mongo document to the API contract.</summary>
public static class AppointmentMapper
{
    public static AppointmentDto ToDto(Appointment appointment) => new()
    {
        Id = appointment.Id,
        CustomerName = appointment.CustomerName,
        CustomerPhone = appointment.CustomerPhone,
        ServiceType = appointment.ServiceType,
        AppointmentDate = appointment.AppointmentDate,
        Status = appointment.Status,
        CreatedAt = appointment.CreatedAt
    };
}
