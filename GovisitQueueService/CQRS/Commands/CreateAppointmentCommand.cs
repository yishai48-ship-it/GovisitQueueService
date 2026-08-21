using GovisitQueueService.Data;
using GovisitQueueService.DTOs;
using GovisitQueueService.Models;
using MediatR;

namespace GovisitQueueService.CQRS.Commands;

/// <summary>Books a new appointment. Returns the generated identifier.</summary>
public record CreateAppointmentCommand(CreateAppointmentDto Appointment) : IRequest<string>;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, string>
{
    private readonly MongoDbContext _context;

    public CreateAppointmentCommandHandler(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = new Appointment
        {
            CustomerName = request.Appointment.CustomerName,
            CustomerPhone = request.Appointment.CustomerPhone,
            ServiceType = request.Appointment.ServiceType,
            AppointmentDate = request.Appointment.AppointmentDate,
            Status = AppointmentStatuses.Scheduled,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Appointments.InsertOneAsync(appointment, cancellationToken: cancellationToken);

        return appointment.Id;
    }
}
