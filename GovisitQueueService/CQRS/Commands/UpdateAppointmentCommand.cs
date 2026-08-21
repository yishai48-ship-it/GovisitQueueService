using GovisitQueueService.Data;
using GovisitQueueService.DTOs;
using GovisitQueueService.Models;
using MediatR;
using MongoDB.Driver;

namespace GovisitQueueService.CQRS.Commands;

/// <summary>Updates an existing appointment. False means no document matched the id.</summary>
public record UpdateAppointmentCommand(string Id, UpdateAppointmentDto Appointment) : IRequest<bool>;

public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, bool>
{
    private readonly MongoDbContext _context;

    public UpdateAppointmentCommandHandler(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var filter = Builders<Appointment>.Filter.Eq(a => a.Id, request.Id);

        var update = Builders<Appointment>.Update
            .Set(a => a.CustomerName, request.Appointment.CustomerName)
            .Set(a => a.CustomerPhone, request.Appointment.CustomerPhone)
            .Set(a => a.ServiceType, request.Appointment.ServiceType)
            .Set(a => a.AppointmentDate, request.Appointment.AppointmentDate)
            .Set(a => a.Status, request.Appointment.Status);

        var result = await _context.Appointments.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        // MatchedCount rather than ModifiedCount: re-submitting identical values is
        // still a successful update, not a 404.
        return result.MatchedCount > 0;
    }
}
